USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE VIEW USELESS_DISCS_VIEW AS
SELECT
	Disc.*
FROM Disc INNER JOIN OperationLog 
	ON Disc.Id = OperationLog.IdDisc
GROUP BY 
	Disc.Id, Disc.Name, Disc.Price
HAVING COUNT(OperationLog.IdDisc) = 0

GO
SELECT * FROM USELESS_DISCS_VIEW