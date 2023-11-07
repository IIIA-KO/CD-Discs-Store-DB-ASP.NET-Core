CREATE PROCEDURE SP_CHANGE_DISC_TYPE_PRICE
	@DiscType VARCHAR(6), 
	@Percent INT, 
	@Increase BIT --/ 1 - підвищити ; 2 - знизити
AS
BEGIN
	IF(@Increase = 1)
	BEGIN
		IF(@DiscType = 'Фільм')
		BEGIN
			UPDATE Disc
			SET
				Price += (Price * @Percent)/100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
		END
		ELSE
		BEGIN
			UPDATE Disc
			SET
				Price += (Price * @Percent)/100
			FROM Disc INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
	END
	ELSE
	BEGIN
		IF(@DiscType = 'Фільм')
		BEGIN
			UPDATE Disc
			SET
				Price -= (Price * @Percent)/100
			FROM Disc INNER JOIN DiscFilm
				ON Disc.Id = DiscFilm.IdDisc
		END
		ELSE
		BEGIN
			UPDATE Disc
			SET
				Price -= (Price * @Percent)/100
			FROM Disc INNER JOIN DiscMusic
				ON Disc.Id = DiscMusic.IdDisc
		END
	END
END
GO