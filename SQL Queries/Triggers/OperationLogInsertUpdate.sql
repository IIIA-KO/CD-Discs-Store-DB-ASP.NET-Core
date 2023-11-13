USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
ALTER TRIGGER TR_OPERATION_LOG_INSERT_UPDATE
ON OperationLog
AFTER INSERT, UPDATE
AS
BEGIN
	INSERT INTO PersonalDiscount (Id, IdClient, StartDateTime, EndDateTime, PersonalDiscountValue)
	SELECT
		NEWID(),
		TEMP_DB.CLIENT_ID,
		TEMP_DB.OperationDateTimeStart,
		TEMP_DB.OperationDateTimeEnd,
		CASE
			WHEN TEMP_DB.OperationType = (SELECT Id FROM OperationType WHERE TypeName = 'Purchase')
			THEN
				(
					CASE
						WHEN TEMP_DB.TOTAL_PROFIT >= 50000	THEN 20
						WHEN TEMP_DB.TOTAL_PROFIT >= 30000	THEN 15
						WHEN TEMP_DB.TOTAL_PROFIT >= 15000	THEN 10
						WHEN TEMP_DB.TOTAL_PROFIT >= 5000	THEN 5
						WHEN TEMP_DB.TOTAL_PROFIT <  5000	THEN 0
						ELSE 0
					END
				)
			ELSE
				(
					CASE
						WHEN TEMP_DB.OperationDateTimeEnd IS NULL THEN 0
						
						WHEN TEMP_DB.TOTAL_PROFIT >= 50000	AND  TEMP_DB.DATE_DIFFERENCE <=	30	THEN 20
						WHEN TEMP_DB.TOTAL_PROFIT >= 50000	AND  TEMP_DB.DATE_DIFFERENCE >	30	THEN 15
						WHEN TEMP_DB.TOTAL_PROFIT >= 30000	AND  TEMP_DB.DATE_DIFFERENCE <=	30	THEN 15
						WHEN TEMP_DB.TOTAL_PROFIT >= 30000	AND  TEMP_DB.DATE_DIFFERENCE >	30	THEN 10
						WHEN TEMP_DB.TOTAL_PROFIT >= 15000	AND  TEMP_DB.DATE_DIFFERENCE <=	30	THEN 10
						WHEN TEMP_DB.TOTAL_PROFIT >= 15000	AND  TEMP_DB.DATE_DIFFERENCE >	30	THEN 5
						WHEN TEMP_DB.TOTAL_PROFIT >= 5000	AND  TEMP_DB.DATE_DIFFERENCE <=	30	THEN 5
						WHEN TEMP_DB.TOTAL_PROFIT >= 5000	AND  TEMP_DB.DATE_DIFFERENCE >	30	THEN 0
						WHEN TEMP_DB.TOTAL_PROFIT < 5000										THEN 0
						ELSE 0
					END
				)
		END AS DISCOUNT
	FROM
	(
		SELECT
			IdClient AS CLIENT_ID,
			(
				(SELECT
					SUM(Disc.Price)
				FROM OperationLog 
						INNER JOIN Client 
							ON IdClient = Client.Id 
						INNER JOIN Disc 
							ON IdDisc = Disc.Id 
				WHERE IdClient = OP.IdClient AND OperationType = (SELECT Id FROM OperationType WHERE TypeName = 'Purchase'))
				+
				(SELECT
					SUM(Disc.Price) / 2
				FROM	OperationLog INNER JOIN Client 
							ON IdClient = Client.Id 
						INNER JOIN Disc 
							ON IdDisc = Disc.Id 
				WHERE IdClient = OP.IdClient AND OperationType = (SELECT Id FROM OperationType WHERE TypeName = 'Rent'))
			) AS TOTAL_PROFIT,
			OperationType,
			OperationDateTimeStart,
			OperationDateTimeEnd,
			DATEDIFF(DAY, OperationDateTimeStart, OperationDateTimeEnd) AS DATE_DIFFERENCE
		FROM inserted AS OP
		GROUP BY IdClient, OperationType, OperationDateTimeStart, OperationDateTimeEnd
	) AS TEMP_DB
	ORDER BY TEMP_DB.CLIENT_ID
END