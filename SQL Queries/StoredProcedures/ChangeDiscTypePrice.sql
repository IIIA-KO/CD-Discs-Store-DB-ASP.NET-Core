USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
CREATE PROCEDURE SP_CHANGE_DISC_TYPE_PRICE
	@DiscType VARCHAR(6), 
	@Percent INT, 
	@Increase BIT --/ 1 - INCREASE / 0 - DECREASE
AS
BEGIN
	IF(@Increase = 1)
	BEGIN
		IF(@DiscType = 'Film')
		BEGIN
			UPDATE Disc
			SET
				Price += (Price * @Percent)/100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
		END
		ELSE IF (@DiscType = 'Music')
		BEGIN
			UPDATE Disc
			SET
				Price += (Price * @Percent)/100
			FROM Disc INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
		ELSE
		BEGIN
			UPDATE Disc
			SET
				Price += (Price * @Percent) / 100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
			INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
	END
	ELSE
	BEGIN
		IF(@DiscType = 'Film')
		BEGIN
			UPDATE Disc
			SET
				Price -= (Price * @Percent)/100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
		END
		ELSE IF (@DiscType = 'Music')
		BEGIN
			UPDATE Disc
			SET
				Price -= (Price * @Percent)/100
			FROM Disc INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
		ELSE
		BEGIN
			UPDATE Disc
			SET
				Price -= (Price * @Percent) / 100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
			INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
	END
END
GO

Select 
	DiscFilm.IdDisc,
	Disc.Price
from Disc inner join DiscFilm
	on Disc.Id = DiscFilm.IdDisc

Exec SP_CHANGE_DISC_TYPE_PRICE 'Film', 45, 1