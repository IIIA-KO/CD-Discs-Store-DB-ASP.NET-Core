use [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE FUNCTION FN_GET_TOTAL_PURCHASE_PROFIT_FROM_CLIENT (@IdClient UNIQUEIDENTIFIER)
RETURNS DECIMAL(9, 2)
AS
BEGIN
    RETURN (SELECT
		SUM(Disc.Price)
	FROM OperationLog 
		INNER JOIN Client 
			ON IdClient = Client.Id 
		INNER JOIN Disc 
			ON IdDisc = Disc.Id 
	WHERE IdClient = @IdClient AND OperationType = (SELECT Id FROM OperationType WHERE TypeName = 'Purchase'))
END