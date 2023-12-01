USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE VIEW ACTIVE_DEBTORS_VIEW AS
SELECT
	Client.Id,
	Client.FirstName + ' ' + Client.LastName AS FullName,
	CAST(DATEADD(Month, 1, OperationLog.OperationDateTimeStart) AS DATE) AS InDebtFrom,
	CAST(GETDATE() AS DATE) AS 'to'
FROM OperationLog
	INNER JOIN Client
		ON OperationLog.idClient = Client.ID
WHERE DATEDIFF(Month, OperationDateTimeStart, GETDATE()) > 1
		AND OperationLog.OperationDateTimeEnd IS NULL
		AND OperationLog.OperationType = (SELECT Id FROM OperationType Where TypeName = 'Rent')
ORDER BY Client.Id, InDebtFrom OFFSET 0 ROWS

GO
SELECT * FROM ACTIVE_DEBTORS_VIEW