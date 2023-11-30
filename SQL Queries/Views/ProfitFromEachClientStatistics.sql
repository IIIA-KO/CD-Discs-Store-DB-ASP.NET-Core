USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE VIEW PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW
AS
WITH CTE_PROFIT_FOR_EACH_PURCHASE_TYPE
AS
(
	SELECT
		Client.Id as ClientId,
		Client.FirstName as ClientName,
		YEAR(OperationDateTimeStart) AS OrderYear,
		CASE
			WHEN (OperationType = (SELECT Id FROM OperationType Where TypeName = 'Purchase')	AND Disc.Id IN(SELECT IdDisc FROM DiscFilm)) THEN 'Film Purchase'
			WHEN (OperationType = (SELECT Id FROM OperationType Where TypeName = 'Rent')		AND Disc.Id IN(SELECT IdDisc FROM DiscFilm)) THEN 'Film Rent'
		END AS PurchaseType,
		CASE
			WHEN OperationType = (SELECT Id FROM OperationType Where TypeName = 'Purchase')
			THEN SUM(Disc.Price)
			ELSE SUM(Disc.Price) / 2
		END AS Profit
	FROM OperationLog INNER JOIN Client 
			ON OperationLog.IdClient = Client.Id 
		INNER JOIN Disc 
			ON OperationLog.IdDisc = Disc.Id 
		INNER JOIN DiscFilm
			ON DiscFilm.IdDisc = Disc.Id
		INNER JOIN Film
			ON Film.Id = DiscFilm.IdFilm
	GROUP BY
		Client.Id, Client.FirstName, YEAR(OperationDateTimeStart), OperationType, Disc.Id
	UNION ALL
	SELECT
		Client.Id as ClientId,
		Client.FirstName as ClientName,
		YEAR(OperationDateTimeStart) AS OrderYear,
		CASE
			WHEN (OperationType = (SELECT Id FROM OperationType Where TypeName = 'Purchase')	AND Disc.Id IN(SELECT IdDisc FROM DiscMusic))	THEN 'Music Purchase'
			WHEN (OperationType = (SELECT Id FROM OperationType Where TypeName = 'Rent')		AND Disc.Id IN(SELECT IdDisc FROM DiscMusic))	THEN 'Music Rent'
		END AS PurchaseType,
		CASE
			WHEN OperationType = (SELECT Id FROM OperationType Where TypeName = 'Purchase')
			THEN SUM(Disc.Price)
			ELSE SUM(Disc.Price) / 2
		END AS Profit
	FROM OperationLog INNER JOIN Client 
			ON OperationLog.IdClient = Client.Id 
		INNER JOIN Disc 
			ON OperationLog.IdDisc = Disc.Id 
		INNER JOIN DiscMusic
			ON DiscMusic.IdDisc = Disc.Id
		INNER JOIN Music
			ON Music.Id = DiscMusic.IdMusic
	GROUP BY
		Client.Id, Client.FirstName, YEAR(OperationDateTimeStart), OperationType, Disc.Id
)
SELECT
	ClientId,
	ClientName,
	OrderYear,
	[Music Purchase], [Music Rent], [Film Purchase], [Film Rent]
FROM CTE_PROFIT_FOR_EACH_PURCHASE_TYPE
PIVOT
(
	Sum(Profit)
	FOR PurchaseType IN ([Music Purchase], [Music Rent], [Film Purchase], [Film Rent])
) as PIVOT_PURCHASE_TYPE_PROFIT
ORDER BY ClientId, ClientName, OrderYear OFFSET 0 ROWS

GO
SELECT * FROM PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW