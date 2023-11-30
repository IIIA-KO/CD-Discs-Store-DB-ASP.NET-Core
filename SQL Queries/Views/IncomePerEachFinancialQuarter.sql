USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE VIEW INCOME_PER_EACH_FINANCIAL_QUARTER_VIEW AS
WITH CTE_PROFIT_PER_EACH_QUARTER
AS
(
	SELECT
		YEAR(OperationDateTimeStart) AS OrderYear,
		CASE
			WHEN DATEPART(Quarter, OperationDateTimeStart) = 1 THEN '1 quarter'
			WHEN DATEPART(Quarter, OperationDateTimeStart) = 2 THEN '2 quarter'
			WHEN DATEPART(Quarter, OperationDateTimeStart) = 3 THEN '3 quarter'
			WHEN DATEPART(Quarter, OperationDateTimeStart) = 4 THEN '4 quarter'
		END AS DateQuarter,
		CASE
			WHEN OperationType = (SELECT Id FROM OperationType Where TypeName = 'Purchase')
			THEN SUM(Disc.Price)
			ELSE SUM(Disc.Price) / 2
		END AS Profit
	FROM OperationLog INNER JOIN Disc
		ON IdDisc = Disc.Id
	GROUP BY GROUPING SETS 
		(
			(YEAR(OperationDateTimeStart)),
			(YEAR(OperationDateTimeStart), DATEPART(QUARTER, OperationDateTimeStart)),
			(YEAR(OperationDateTimeStart), OperationType)
		)
)
SELECT
	OrderYear,
	[1 quarter], [2 quarter], [3 quarter], [4 quarter]
FROM CTE_PROFIT_PER_EACH_QUARTER
PIVOT
(
	Sum(profit)
	FOR DateQuarter IN ([1 quarter], [2 quarter], [3 quarter], [4 quarter])
) as PIVOT_QUARTER_PROFIT

GO
SELECT * FROM INCOME_PER_EACH_FINANCIAL_QUARTER_VIEW