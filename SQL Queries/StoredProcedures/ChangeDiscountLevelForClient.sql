USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE PROCEDURE SP_CHANGE_DISCOUNT_LEVEL_FOR_CLIENT
	@IdClient UNIQUEIDENTIFIER,
	@Increase BIT --/ 1 - INCREASE / 0 - DECREASE
AS
BEGIN
	BEGIN
		IF(@Increase = 1)
		BEGIN
			IF (SELECT TOP 1 PersonalDiscountValue FROM PersonalDiscount WHERE IdClient = @IdClient) <= 15
			BEGIN
				UPDATE PersonalDiscount
				SET PersonalDiscountValue += 5
				WHERE IdClient = @IdClient
			END
		END
		ELSE
		BEGIN
			IF (SELECT TOP 1 PersonalDiscountValue FROM PersonalDiscount WHERE IdClient = @IdClient) >= 5
			BEGIN
				UPDATE PersonalDiscount
				SET PersonalDiscountValue -= 5
				WHERE IdClient = @IdClient
			END
		END
	END
END
GO

SELECT
	TOP 1 PersonalDiscountValue 
FROM PersonalDiscount 
WHERE idClient = 'D7BE63E5-0A4F-4DA4-ABE7-02648D8EE65C'
ORDER BY StartDateTime DESC

select * from Client