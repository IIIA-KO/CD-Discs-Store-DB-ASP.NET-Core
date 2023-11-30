USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE VIEW ADULT_FILM_RATIO_BY_MONTH_VIEW
AS
WITH CTE_RATIO_PER_EACH_MONTH
AS
(
	SELECT
		*
	FROM
	(
		SELECT
			YEAR(OperationDateTimeStart) AS OrderYear,
			'M' as Sex,
			CASE
				WHEN MONTH(OperationDateTimeStart) = 1 THEN 'January'
				WHEN MONTH(OperationDateTimeStart) = 2 THEN 'February'
				WHEN MONTH(OperationDateTimeStart) = 3 THEN 'March'
				WHEN MONTH(OperationDateTimeStart) = 4 THEN 'April'
				WHEN MONTH(OperationDateTimeStart) = 5 THEN 'May'
				WHEN MONTH(OperationDateTimeStart) = 6 THEN 'June'
				WHEN MONTH(OperationDateTimeStart) = 7 THEN 'July'
				WHEN MONTH(OperationDateTimeStart) = 8 THEN 'August'
				WHEN MONTH(OperationDateTimeStart) = 9 THEN 'September'
				WHEN MONTH(OperationDateTimeStart) = 10 THEN 'October'
				WHEN MONTH(OperationDateTimeStart) = 11 THEN 'November'
				WHEN MONTH(OperationDateTimeStart) = 12 THEN 'December'
			END AS DateMonth,
			(
				(Select 
					(COUNT(*) * 100)
					/
					(Select 
						COUNT(*)
					FROM OperationLog INNER JOIN Disc
							ON IdDisc = Disc.Id
						INNER JOIN Client
							ON OperationLog.IdClient = Client.Id
						INNER JOIN DiscFilm
							ON DiscFilm.IdDisc = Disc.Id
						INNER JOIN Film
							ON Film.Id = DiscFilm.IdFilm)
				FROM OperationLog INNER JOIN Disc
						ON IdDisc = Disc.Id
					INNER JOIN Client
						ON OperationLog.IdClient = Client.Id
					INNER JOIN DiscFilm
						ON DiscFilm.IdDisc = Disc.Id
					INNER JOIN Film
						ON Film.Id = DiscFilm.IdFilm
				WHERE Film.AgeLimit >= 18 AND Client.Sex = 1)
			) as AdultRatePercent
		FROM OperationLog INNER JOIN Disc
				ON IdDisc = Disc.Id
			INNER JOIN Client
				ON OperationLog.IdClient = Client.Id
			INNER JOIN DiscFilm
				ON DiscFilm.IdDisc = Disc.Id
			INNER JOIN Film
				ON Film.Id = DiscFilm.IdFilm
		WHERE Client.Sex = 1
		GROUP BY GROUPING SETS 
			(
				(YEAR(OperationDateTimeStart)),
				(YEAR(OperationDateTimeStart), MONTH(OperationDateTimeStart)),
				(YEAR(OperationDateTimeStart), OperationType)
			)

		UNION

		SELECT
			YEAR(OperationDateTimeStart) AS OrderYear,
			'F' as Sex,
			CASE
				WHEN MONTH(OperationDateTimeStart) = 1 THEN 'January'
				WHEN MONTH(OperationDateTimeStart) = 2 THEN 'February'
				WHEN MONTH(OperationDateTimeStart) = 3 THEN 'March'
				WHEN MONTH(OperationDateTimeStart) = 4 THEN 'April'
				WHEN MONTH(OperationDateTimeStart) = 5 THEN 'May'
				WHEN MONTH(OperationDateTimeStart) = 6 THEN 'June'
				WHEN MONTH(OperationDateTimeStart) = 7 THEN 'July'
				WHEN MONTH(OperationDateTimeStart) = 8 THEN 'August'
				WHEN MONTH(OperationDateTimeStart) = 9 THEN 'September'
				WHEN MONTH(OperationDateTimeStart) = 10 THEN 'October'
				WHEN MONTH(OperationDateTimeStart) = 11 THEN 'November'
				WHEN MONTH(OperationDateTimeStart) = 12 THEN 'December'
			END AS DateMonth,
			(
				(Select 
					(COUNT(*) * 100)
					/
					(Select 
						COUNT(*)
					FROM OperationLog INNER JOIN Disc
							ON IdDisc = Disc.Id
						INNER JOIN Client
							ON OperationLog.IdClient = Client.Id
						INNER JOIN DiscFilm
							ON DiscFilm.IdDisc = Disc.Id
						INNER JOIN Film
							ON Film.Id = DiscFilm.IdFilm)
				FROM OperationLog INNER JOIN Disc
						ON IdDisc = Disc.Id
					INNER JOIN Client
						ON OperationLog.IdClient = Client.Id
					INNER JOIN DiscFilm
						ON DiscFilm.IdDisc = Disc.Id
					INNER JOIN Film
						ON Film.Id = DiscFilm.IdFilm
				WHERE Film.AgeLimit >= 18 AND Client.Sex = 0)
			) as AdultRatePercent
		FROM OperationLog INNER JOIN Disc
				ON IdDisc = Disc.Id
			INNER JOIN Client
				ON OperationLog.IdClient = Client.Id
			INNER JOIN DiscFilm
				ON DiscFilm.IdDisc = Disc.Id
			INNER JOIN Film
				ON Film.Id = DiscFilm.IdFilm
		WHERE Client.Sex = 0
		GROUP BY GROUPING SETS 
			(
				(YEAR(OperationDateTimeStart)),
				(YEAR(OperationDateTimeStart), MONTH(OperationDateTimeStart)),
				(YEAR(OperationDateTimeStart), OperationType)
			)
	) AS RESULT
	ORDER BY OrderYear OFFSET 0 ROWS
)
SELECT
	OrderYear,
	Sex,
	CAST([January]		AS VARCHAR(20))	+ '%' AS January,
    CAST([February]		AS VARCHAR(20))	+ '%' AS February,
    CAST([March]		AS VARCHAR(20))	+ '%' AS March,
    CAST([April]		AS VARCHAR(20))	+ '%' AS April,
    CAST([May]			AS VARCHAR(20))	+ '%' AS May,
    CAST([June]			AS VARCHAR(20))	+ '%' AS June,
    CAST([July]			AS VARCHAR(20))	+ '%' AS July,
    CAST([August]		AS VARCHAR(20))	+ '%' AS August,
    CAST([September]	AS VARCHAR(20))	+ '%' AS September,
    CAST([October]		AS VARCHAR(20))	+ '%' AS October,
    CAST([November]		AS VARCHAR(20))	+ '%' AS November,
    CAST([December]		AS VARCHAR(20))	+ '%' AS December
FROM CTE_RATIO_PER_EACH_MONTH
PIVOT
(
	Sum(AdultRatePercent)
	FOR DateMonth IN (January, February, March, April, May, June, July, August, September, October, November, December)
) as PIVOT_MONTH_RATIO

GO
SELECT * FROM ADULT_FILM_RATIO_BY_MONTH_VIEW