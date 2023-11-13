use [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

-----------------------------------------Client DATA GENERATION-----------------------------------------
GO
------------------RANDOM DATE GENERATION------------------
DECLARE @StartDate AS DATE;
DECLARE @EndDate AS DATE;

SELECT @StartDate = '01/01/1970',
       @EndDate = GETDATE();
------------------RANDOM DATE DENERATION------------------
BEGIN 
	DECLARE @MetaClient TABLE 
	(
		M_Id INT,
		M_FirstName NVARCHAR(50),
		M_LastName NVARCHAR(50),
		M_AddressPart NVARCHAR (50),
		M_City NVARCHAR(50),
		M_ContactPhone VARCHAR(3),
		M_ContactMail NVARCHAR(100),
		M_BirthDay DATE,
		M_MarriedStatus BIT,
		M_Sex BIT,
		M_Child BIT
	)

	DECLARE @number INT,
	@FirstName NVARCHAR(50), @LastName NVARCHAR(50), 
	@Address NVARCHAR (100), @City NVARCHAR (50), 
	@ContactPhone NVARCHAR(20), @ContactMail NVARCHAR(100), @BirthDay DATE,
	@MarriedStatus BIT, @Sex BIT, @Child BIT 

	SET @number = 100;
	INSERT INTO @MetaClient
	VALUES(1, N'Степан',	N'Степаненко',		N'Бандери',			N'Житомир',		'073', '@gmail.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (2, N'Михайло',	N'Михайленко',		N'Грушевського',	N'Київ',		'075', '@icloud.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (3, N'Іван',		N'Іваненко',		N'Мазепи',			N'Львів',		'096', '@host.org',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (4, N'Петро',		N'Петренко',		N'Скоропадського',	N'Луцьк',		'095', '@mail.org',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (5, N'Богдан',	N'Богданенко',		N'Хмельницького',	N'Хмельницьк',	'098', '@ukr.net',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (6, N'Антон',		N'Антоненко',		N'Сікорського',		N'Ужгород',		'063', '@yahoo.net',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (7, N'Олександр',	N'Олександренко',	N'Українки',		N'Полтава',		'067', '@tele.info',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (8, N'Тарас',		N'Тарасенко',		N'Шевченка',		N'Чернігів',	'068', '@web.info',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)))

	WHILE @number > 0
    BEGIN
		SELECT @FirstName =		M_FirstName		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @LastName =		M_LastName		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Address =		M_AddressPart	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @City =			M_City			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @ContactPhone =	M_ContactPhone	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @ContactMail =	M_ContactMail	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @BirthDay =		M_Birthday		FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @MarriedStatus =	M_MarriedStatus	FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Sex =			M_Sex			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @Child =			M_Child			FROM @MetaClient WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)

		SET @Address = N'вулиця' + ' ' + @Address + ' ' + CAST((FLOOR(RAND()*(100-1+1)+1)) AS VARCHAR(3))
		SET @ContactPhone = @ContactPhone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))
		SET @ContactMail = LOWER(@FirstName) + '.' + LOWER(@LastName) + @ContactMail
		SET @number -= 1

		INSERT INTO Client (FirstName, LastName, [Address], City, ContactPhone, ContactMail, BirthDay, MarriedStatus, Sex, HasChild)
		VALUES (@FirstName, @LastName, @Address, @City, @ContactPhone, @ContactMail, @BirthDay, @MarriedStatus, @Sex, @Child)
    END
END

SELECT * FROM Client
-----------------------------------------Client DATA GENERATION-----------------------------------------





-----------------------------------------PhoneList DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaPhoneList TABLE
	(
		M_Id INT,
		M_Phone NVARCHAR(20),
		M_IdClient UNIQUEIDENTIFIER
	)

	DECLARE @number INT,  @Phone NVARCHAR(20), @IdClient UNIQUEIDENTIFIER

	SET @number = 40
	INSERT INTO @MetaPhoneList
	VALUES	(1, '073', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(2, '075', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(3, '096', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(4, '095', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(5, '098', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(6, '063', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(7, '067', (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
			(8, '068', (SELECT TOP 1 Id FROM Client ORDER BY NEWID()))

	WHILE @number > 0
	BEGIN
		SELECT @Phone = M_Phone FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @IdClient = M_IdClient FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
	
		SET @Phone = @Phone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))

		INSERT INTO PhoneList (Phone, IdClient)
		VALUES (@Phone, @IdClient)

		SET @number -= 1
	END
END

SELECT * FROM PhoneList
-----------------------------------------PhoneList DATA GENERATION-----------------------------------------





-----------------------------------------MailList DATA GENERATION-----------------------------------------
GO
BEGIN
    DECLARE @MetaMailList TABLE
    (
        M_Id INT,
        M_Mail NVARCHAR(100),
        M_IdClient UNIQUEIDENTIFIER
    )

    DECLARE @number INT,  @Mail NVARCHAR(100), @IdClient UNIQUEIDENTIFIER

    SET @number = 40
    INSERT INTO @MetaMailList
    VALUES  (1, '@gmail.com',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (2, '@icloud.com',  (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (3, '@host.org',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (4, '@mail.org',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (5, '@ukr.net',     (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (6, '@yahoo.net',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (7, '@tele.info',   (SELECT TOP 1 Id FROM Client ORDER BY NEWID())),
            (8, '@web.info',    (SELECT TOP 1 Id FROM Client ORDER BY NEWID()))

    WHILE @number > 0
    BEGIN
        SELECT @Mail = M_Mail, @IdClient = M_IdClient FROM @MetaMailList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
        
        SET @Mail = LOWER((SELECT FirstName FROM Client WHERE Id = @IdClient)) + '.' + LOWER((SELECT LastName FROM Client WHERE Id = @IdClient)) + @Mail

        INSERT INTO MailList(Mail, IdClient)
        VALUES (@Mail, @IdClient)

        SET @number -= 1
    END
END


SELECT * FROM MailList
-----------------------------------------MailList DATA GENERATION-----------------------------------------





-----------------------------------------Music DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaMusic TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Genre NVARCHAR(50),
		M_Artist NVARCHAR(50),
		M_Language NVARCHAR(50)
	)
	DECLARE @number INT, @Name NVARCHAR(50), @Genre NVARCHAR(50), @Artist NVARCHAR(50), @Language NVARCHAR(50)

	SET @number = 40;
	INSERT INTO @MetaMusic
	VALUES  (1,  'Steps Of The Storm',					'Pop',			'High Fire',				'English'),
			(2,  'Think For A Moment Of Peace',			'Rock',			'Echo Chords',				'Spanish'),
			(3,  'Street',								'Hip-Hop',		'Simple Spark',				'Fhench'),
			(4,  'Memories',							'Metal',		'Lost Aces',				'German'),
			(5,  'Dreams Of The Devil',					'Jazz',			'Adorable Enemies',			'Italian'),
			(6,  'Way Of Heaven',						'Blues',		'Glass Union',				'Ukrainian'),
			(7,  'Tracks Of My Angel',					'Country',		'Flux',						'Polish'),
			(8,  'Living Of My Enemies',				'EDM',			'Spoof',					'Arabic'),
			(9,  'Desired And Angel',					'Latin',		'Gesture',					'Japanese'),
			(10, 'Wicked And Soul',						'R&B',			'Rapture',					'Korean'),
			(11, 'You Knock Me Off My Feet',			'Folk',			'Strife',					'Hindi'),
			(12, 'It`s Time For Rock And Roll',			'World',		'Oasis of Integrity',		'Chinese'),
			(13, 'Get It Together',						'New Age',		'Cipher of Doubt',			'Welsh'),
			(14, 'Babe, I`m Lonely',					'Acoustic',		'Salvo of Rage',			'English'),
			(15, 'Rock My World',						'Pop',			'Figures of One Night',		'Spanish'),
			(16, 'She Thinks I Rock All Night',			'Rock',			'Marvel of Habit',			'Fhench'),
			(17, 'He Hopes I`m Nothing Without You',	'Hip-Hop',		'Season of Obscurity',		'German'),
			(18, 'She Knows You Called For Me',			'Metal',		'Association of Utopia',	'Italian'),
			(19, 'She Hopes I Live On The Wild Side',	'Jazz',			'Theory',					'Ukrainian'),
			(20, 'I Go My Own Way',						'Blues',		'Delirium',					'Polish'),
			(21, 'She Thinks He`s Going To Hell',		'Country',		'Warmth of Velocity',		'Arabic'),
			(23, 'She Knows He`s Rock`N Roll',			'EDM',			'Epoch of Fiction',			'Japanese'),
			(24, 'She Hates You Rock My World',			'Latin',		'Personality of Luck',		'Korean'),
			(25, 'He Thinks He`s Going To Hell',		'R&B',			'Dynamite Sounds',			'Hindi')

	WHILE @number > 0
	BEGIN
		SELECT @Name =		M_Name		FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Genre =		M_Genre		FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Artist =	M_Artist	FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)
		SELECT @Language =	M_Language	FROM @MetaMusic WHERE M_Id =  FLOOR(RAND()*(25-1+1)+1)

		INSERT INTO Music (Name, Genre, Artist, Language)
		VALUES (@Name, @Genre, @Artist, @Language)

		SET @number -= 1
	END
END

SELECT * FROM Music
-----------------------------------------Music DATA GENERATION-----------------------------------------





-----------------------------------------Film  DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaFilm TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Genre NVARCHAR(30),
		M_Producer NVARCHAR(50),
		M_MainRole NVARCHAR(50),
		M_AgeLimit INT
	)
	DECLARE @number INT, @Id INT, @Name NVARCHAR(50), @Genre NVARCHAR(50), @Producer NVARCHAR(50), @MainRole NVARCHAR(50), @AgeLimit INT

	SET @number = 40
	INSERT INTO @MetaFilm
	VALUES  (1,		'warrior without desire',		'Classic',		'Maximo Wheeler',		'Brynlee Rojas',		6),
			(2,		'parrot of reality',			'Drama',		'Jerry Buck',			'Serena Mcdowell',		12),
			(3,		'criminals without courage',	'Thriller',		'Tyshawn Frost',		'Jazlyn Griffin',		14),
			(4,		'heirs of the ancestors',		'Action',		'Heath Mckinney',		'Urijah Trevino',		16),
			(5,		'creators and owls',			'Comedy',		'Rafael Rich',			'Natalie Shah',			18),
			(6,		'kings and priests',			'Romance',		'Kailey Sellers',		'Nathanael Cordova',	0),
			(7,		'victory of the river',			'Musical',		'Bradyn Mckenzie',		'Ella Landry',			6),
			(8,		'influence of darkness',		'Animation',	'Kael Boyer',			'Carsen Rose',			12),
			(9,		'bound to my school',			'Horror',		'Shamar Love',			'Vaughn Cherry',		14),
			(10,	'bathing in the world',			'Foreign',		'Ryan Gonzalez',		'Isiah Curtis',			16),
			(11,	'dead in dreams',				'Independent',	'Yusuf Horton',			'Alicia Cervantes',		18),
			(12,	'dead in the city',				'Documentary',	'Kassandra Raymond',	'Maximillian Collier',	0),
			(13,	'blood at the river',			'Classic',		'Helen Krause',			'Richard Bautista',		6),
			(14,	'sounds in the city',			'Drama',		'Ally Alvarado',		'Jaylen Barajas',		12),
			(15,	'faith of nightmares',			'Thriller',		'Lorenzo Bishop',		'Caden Mcbride',		14),
			(16,	'meeting in the immortals',		'Action',		'Zaid Newman',			'Jamarcus Garner',		16),
			(17,	'eliminating my family',		'Comedy',		'Deacon Baker',			'Casey Wolfe',			18),
			(18,	'songs of the north',			'Romance',		'Ainsley Hernandez',	'Craig Tapia',			0),
			(19,	'destroying nature',			'Musical',		'Gilbert Mcdowell',		'Kaliyah Harrell',		6),
			(20,	'answering the south',			'Animation',	'Evan Hobbs',			'Danica Bullock',		12),
			(21,	'praised by the south',			'Horror',		'Madden Ewing',			'Deacon Estrada',		14),
			(22,	'the time of empire',			'Foreign',		'Alisa Sanford',		'Jagger Solomon',		16),
			(23,	'dancing in the commander',		'Independent',	'Judith Le',			'Nataly Poole',			18),
			(24,	'mending eternity',				'Documentary',	'Marc Horton',			'Glenn Barnett',		0),
			(25,	'rescue in the dark',			'Classic',		'Giovanni Sanders',		'Belen Chase',			6)

	WHILE @number > 0
	BEGIN
		SELECT @Name =		M_Name		FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @Genre =		M_Genre		FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @Producer =	M_Producer	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @MainRole =	M_MainRole	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)
		SELECT @AgeLimit =	M_AgeLimit	FROM @MetaFilm WHERE M_Id =	FLOOR(RAND()*(25-1+1)+1)

		INSERT INTO Film (Name, Genre, Producer, MainRole, AgeLimit)
		VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit);

		SET @number -= 1
	END
END

SELECT * FROM Film
-----------------------------------------Film  DATA GENERATION-----------------------------------------





-----------------------------------------Disc DATA GENERATION-----------------------------------------
GO
BEGIN
	DECLARE @MetaDisc TABLE
	(
		M_Id INT,
		M_Name NVARCHAR(50),
		M_Price DECIMAL(9, 2)
	)

	DECLARE @number INT, @Name NVARCHAR(50), @Price DECIMAL(9, 2)

	SET @number = 200;
	INSERT INTO @MetaDisc
	VALUES  (1, 'Film1', RAND()*(50-5+1)+6),
			(2, 'Film2', RAND()*(50-5+1)+6),
			(3, 'Film3', RAND()*(50-5+1)+6),
			(4, 'Film4', RAND()*(50-5+1)+6),
			(5, 'Film5', RAND()*(50-5+1)+6),
			(6, 'Film6', RAND()*(50-5+1)+6),
			(7, 'Film7', RAND()*(50-5+1)+6),
			(8, 'Film8', RAND()*(50-5+1)+6),
			(9, 'Film9', RAND()*(50-5+1)+6),
			(10, 'Film10', RAND()*(50-5+1)+6),
			(11, 'Film11', RAND()*(50-5+1)+6),
			(12, 'Film12', RAND()*(50-5+1)+6),
			(13, 'Film13', RAND()*(50-5+1)+6),
			(14, 'Film14', RAND()*(50-5+1)+6),
			(15, 'Film15', RAND()*(50-5+1)+6),
			(16, 'Film16', RAND()*(50-5+1)+6),
			(17, 'Film17', RAND()*(50-5+1)+6),
			(18, 'Film18', RAND()*(50-5+1)+6),
			(19, 'Film19', RAND()*(50-5+1)+6),
			(20, 'Film20', RAND()*(50-5+1)+6),
			(21, 'Music1', RAND()*(50-5+1)+6),
			(22, 'Music2', RAND()*(50-5+1)+6),
			(23, 'Music3', RAND()*(50-5+1)+6),
			(24, 'Music4', RAND()*(50-5+1)+6),
			(25, 'Music5', RAND()*(50-5+1)+6),
			(26, 'Music6', RAND()*(50-5+1)+6),
			(27, 'Music7', RAND()*(50-5+1)+6),
			(28, 'Music8', RAND()*(50-5+1)+6),
			(29, 'Music9', RAND()*(50-5+1)+6),
			(30, 'Music10', RAND()*(50-5+1)+6),
			(31, 'Music11', RAND()*(50-5+1)+6),
			(32, 'Music12', RAND()*(50-5+1)+6),
			(33, 'Music13', RAND()*(50-5+1)+6),
			(34, 'Music14', RAND()*(50-5+1)+6),
			(35, 'Music15', RAND()*(50-5+1)+6),
			(36, 'Music16', RAND()*(50-5+1)+6),
			(37, 'Music17', RAND()*(50-5+1)+6),
			(38, 'Music18', RAND()*(50-5+1)+6),
			(39, 'Music19', RAND()*(50-5+1)+6),
			(40, 'Music20', RAND()*(50-5+1)+6)

	WHILE @number > 0
	BEGIN
		SELECT @Name =	M_Name	FROM @MetaDisc WHERE M_Id = FLOOR(RAND()*(40-1+1)+1)
		SELECT @Price = M_Price FROM @MetaDisc WHERE M_Id = FLOOR(RAND()*(40-1+1)+1)

		INSERT INTO Disc (Name, Price)
		VALUES (@Name, @Price)

		SET @number -=1
	END
END

SELECT * FROM Disc
-----------------------------------------Disc DATA GENERATION-----------------------------------------





-----------------------------------------DiscMusic DATA GENERATION-----------------------------------------
GO

DECLARE @DiscId UNIQUEIDENTIFIER, @MusicId UNIQUEIDENTIFIER

DECLARE disc_cursor CURSOR FOR 
SELECT Id FROM Disc
WHERE Name LIKE '%Music%'

OPEN disc_cursor

FETCH NEXT FROM disc_cursor INTO @DiscId

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @MusicId = (SELECT TOP 1 Id FROM Music ORDER BY NEWID())

	INSERT INTO DiscMusic (IdDisc, IdMusic)
	VALUES (@DiscId, @MusicId)

	FETCH NEXT FROM disc_cursor INTO @DiscId
END

CLOSE disc_cursor
DEALLOCATE disc_cursor

DELETE FROM DiscMusic
SELECT * FROM DiscMusic ORDER BY IdDisc
-----------------------------------------DiscMusic DATA GENERATION-----------------------------------------





-----------------------------------------DiscFilm DATA GENERATION-----------------------------------------
GO

DECLARE @DiscId UNIQUEIDENTIFIER, @FilmId UNIQUEIDENTIFIER

DECLARE disc_cursor CURSOR FOR 
SELECT Id FROM Disc
SELECT Id FROM Disc
WHERE Name LIKE '%Film%'

OPEN disc_cursor

FETCH NEXT FROM disc_cursor INTO @DiscId

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @FilmId = (SELECT TOP 1 Id FROM Film ORDER BY NEWID())

	INSERT INTO DiscFilm (IdDisc, IdFilm)
	VALUES (@DiscId, @FilmId)

	FETCH NEXT FROM disc_cursor INTO @DiscId
END

CLOSE disc_cursor
DEALLOCATE disc_cursor

DELETE FROM DiscFilm
SELECT * FROM DiscFilm ORDER BY IdDisc
-----------------------------------------DiscFilm DATA GENERATION-----------------------------------------






-----------------------------------------OperationType DATA GENERATION-----------------------------------------
INSERT INTO OperationType(TypeName)
VALUES ('Purchase'), ('Rent')

SELECT * FROM OperationType
-----------------------------------------OperationType DATA GENERATION-----------------------------------------





-----------------------------------------OperationLog DATA GENERATION-----------------------------------------
GO
------------------RANDOM DATE GENERATION------------------
DECLARE @StartDate AS DATE;
DECLARE @EndDate AS DATE;

SELECT @StartDate = '01/01/2014',
       @EndDate = DATEADD(DAY, -60, GETDATE());
------------------RANDOM DATE GENERATION------------------

BEGIN
	DECLARE @number INT = 150000, @OperationType UNIQUEIDENTIFIER, @OperationStart DATETIME, @OperationEnd DATETIME, @IdClient UNIQUEIDENTIFIER, @IdDisc UNIQUEIDENTIFIER

	WHILE @number > 0
	BEGIN
		SET @OperationType = (SELECT TOP 1 Id FROM OperationType ORDER BY NEWID())
	
		SET @OperationStart = DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate)

		DECLARE @RandomDebtor INT = FLOOR(RAND()*(10-1+1)+1)
		SET @OperationEnd =	
			CASE
				WHEN (SELECT TypeName FROM OperationType WHERE Id = @OperationType) = 'Purchase'
				THEN NULL
				ELSE
					(
						CASE
							WHEN @RandomDebtor = FLOOR(RAND()*(10-1+1)+1) --Орендар, що не повернув диск визначається рандомно, кожен десятий може бути таким
							THEN NULL --Клієнт орендував диск, але не повернув
							ELSE DATEADD(DAY, FLOOR(RAND()*(80-20+1)+20), @OperationStart)
						END
					)
			END

		SET @IdClient = (SELECT TOP 1 Id FROM Client ORDER BY NEWID())
		SET @IdDisc = (SELECT TOP 1 Id FROM Disc ORDER BY NEWID())

		INSERT INTO OperationLog (OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdClient, IdDisc)
		VALUES (@OperationType, @OperationStart, @OperationEnd, @IdClient, @IdDisc)

		SET @number -= 1
	END
END

SELECT *, DATEDIFF(DAY, OperationDateTimeStart, OperationDateTimeEnd) AS DATEDIFF FROM OperationLog order by IdClient

select * from OperationLog
select * from PersonalDiscount
-----------------------------------------OperationLog DATA GENERATION-----------------------------------------