use [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE PROCEDURE SP_CHANGE_DISCOUNT_LEVEL_FOR_CLIENT
	@IdClient UNIQUEIDENTIFIER,
	@Increase BIT --/ 1 - підвищити / 2 - знизити
AS
BEGIN
	BEGIN
		IF(@Increase = 1)
		BEGIN
			IF (SELECT PersonalDiscountValue FROM PersonalDiscount WHERE IdClient = @IdClient) <= 15
			BEGIN
				UPDATE PersonalDiscount
				SET PersonalDiscountValue += 5
				WHERE IdClient = @IdClient
			END
		END
		ELSE
		BEGIN
			IF (SELECT PersonalDiscountValue FROM PersonalDiscount WHERE IdClient = @IdClient) >= 5
			BEGIN
				UPDATE PersonalDiscount
				SET PersonalDiscountValue -= 5
				WHERE IdClient = @IdClient
			END
		END
	END
END
GO