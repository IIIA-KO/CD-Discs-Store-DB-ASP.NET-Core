use [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Client-----------------------------------------
GO
------------------ГЕНЕРАЦІЯ РАНДОМНОЇ ДАТИ------------------
DECLARE @StartDate AS DATE;
DECLARE @EndDate AS DATE;

SELECT @StartDate = '01/01/1960',
       @EndDate = GETDATE();
------------------ГЕНЕРАЦІЯ РАНДОМНОЇ ДАТИ------------------
BEGIN 
	DECLARE @MetaClient TABLE 
	(
		M_Id INT,
		M_FirstName NVARCHAR(50),
		M_LastName NVARCHAR(50),
		M_AddressPart VARCHAR (50),
		M_City VARCHAR(50),
		M_ContactPhone VARCHAR(3),
		M_ContactMail NVARCHAR(100),
		M_BirthDay DATE,
		M_MarriedStatus BIT,
		M_Sex BIT,
		M_Child BIT
	)

	DECLARE @number INT,
	@FirstName NVARCHAR(50), @LastName NVARCHAR(50), @Address NVARCHAR (100), @City NVARCHAR (50), @ContactPhone VARCHAR(3), @ContactMail NVARCHAR(100), @BirthDay DATE,
	@MarriedStatus BIT, @Sex BIT, @Child BIT 

	SET @number = 100;
	INSERT INTO @MetaClient
	VALUES(1, 'Степан',		'Степаненко',	'Бандери',			'Житомир',		'073', '@gmail.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (2, 'Михайло',	'Михайленко',	'Грушевського',		'Київ',			'075', '@icloud.com',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (3, 'Іван',		'Іваненко',		'Мазепи',			'Львів',		'096', '@itstep.org',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (4, 'Петро',		'Петренко',		'Скоропадського',	'Луцьк',		'095', '@mail.org',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (5, 'Богдан',		'Богданенко',	'Хмельницького',	'Хмельницьк',	'098', '@ukr.net',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (6, 'Антон',		'Антоненко',	'Сікорського',		'Ужгород',		'063', '@yahoo.net',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (7, 'Олександр',	'Олександренко','Українки',			'Полтава',		'067', '@tele.info',	DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0))),
		  (8, 'Тарас',		'Тарасенко',	'Шевченка',			'Чернігів',		'068', '@web.info',		DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)), CONVERT(bit, ROUND(1*RAND(),0)))

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

		SET @Address = 'вулиця' + ' ' + @Address + ' ' + CAST((FLOOR(RAND()*(100-1+1)+1)) AS VARCHAR(3))
		SET @ContactPhone = @ContactPhone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))
		SET @ContactMail = LOWER(@FirstName) + '.' + LOWER(@LastName) + @ContactMail
		SET @number -= 1

		INSERT INTO Client 
		VALUES (@FirstName, @LastName, @Address, @City, @ContactPhone, @ContactMail, @BirthDay, @MarriedStatus, @Sex, @Child)
    END
END

SELECT * FROM Client
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Client-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ PhoneList-----------------------------------------
GO
BEGIN
	DECLARE @MetaPhoneList TABLE
	(
		M_Id INT,
		M_Phone varCHAR(15),
		M_IdClient INT
	)

	DECLARE @number INT,  @Phone varCHAR(15), @IdClient INT

	SET @number = 40
	INSERT INTO @MetaPhoneList
	VALUES	(1, '073', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(2, '075', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(3, '096', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(4, '095', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(5, '098', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(6, '063', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(7, '067', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(8, '068', FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1))

	WHILE @number > 0
	BEGIN
		SELECT @Phone = M_Phone FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @IdClient = M_IdClient FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
	
		SET @Phone = @Phone + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(999-100+1)+100)) AS VARCHAR(3)) + CAST((FLOOR(RAND()*(9-0+1)+0)) AS VARCHAR(3))

		INSERT INTO PhoneList
		VALUES (@Phone, @IdClient)

		SET @number -= 1
	END
END

SELECT * FROM PhoneList
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ PhoneList-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ MailList-----------------------------------------
GO
BEGIN
	DECLARE @MetaPhoneList TABLE
	(
		M_Id INT,
		M_Mail CHAR(100),
		M_IdClient INT
	)

	DECLARE @number INT,  @Mail CHAR(100), @IdClient INT

	SET @number = 40
	INSERT INTO @MetaPhoneList
	VALUES	(1, '@gmail.com',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(2, '@icloud.com',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(3, '@itstep.org',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(4, '@mail.org',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(5, '@ukr.net',		 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(6, '@yahoo.net',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(7, '@tele.info',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1)),
			(8, '@web.info',	 FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1))

	WHILE @number > 0
	BEGIN
		SELECT @Mail = M_Mail FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		SELECT @IdClient = M_IdClient FROM @MetaPhoneList WHERE M_Id = FLOOR(RAND()*(8-1+1)+1)
		
		SET @Mail = LOWER((SELECT FirstName FROM Client WHERE Id =  FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1))) + '.' + LOWER((SELECT LastName FROM Client WHERE Id =  FLOOR(RAND()*((SELECT MAX(Id) FROM Client)-1+1)+1))) + @Mail

		INSERT INTO MailList
		VALUES (@Mail, @IdClient)

		SET @number -= 1
	END
END

SELECT * FROM MailList
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ MailList-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Music-----------------------------------------
GO
BEGIN
	DECLARE @MetaMusic TABLE
	(
		M_Id INT,
		M_Name VARCHAR(50),
		M_Genre VARCHAR(50),
		M_Artist VARCHAR(50),
		M_Language VARCHAR(50)
	)
	DECLARE @number INT, @Name VARCHAR(50), @Genre VARCHAR(50), @Artist VARCHAR(50), @Language VARCHAR(50)

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

		INSERT INTO Music
		VALUES (@Name, @Genre, @Artist, @Language)

		SET @number -= 1
	END
END

SELECT * FROM Music
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Music-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Film-----------------------------------------
GO
BEGIN
	DECLARE @MetaFilm TABLE
	(
		M_Id INT,
		M_Name VARCHAR(30),
		M_Genre VARCHAR(30),
		M_Producer VARCHAR(30),
		M_MainRole VARCHAR(30),
		M_AgeLimit INT
	)
	DECLARE @number INT, @Id INT, @Name VARCHAR(30), @Genre VARCHAR(30), @Producer VARCHAR(30), @MainRole VARCHAR(30), @AgeLimit INT

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

		INSERT INTO Film
		VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit);

		SET @number -= 1
	END
END

SELECT * FROM Film
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Film-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Disc-----------------------------------------
GO
BEGIN
	DECLARE @MetaDisc TABLE
	(
		M_Id INT,
		M_Name VARCHAR(50),
		M_Price DECIMAL(9, 2)
	)

	DECLARE @number INT, @Name VARCHAR(50), @Price DECIMAL(9, 2)

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

		INSERT INTO Disc
		VALUES (@Name, @Price)

		SET @number -=1
	END
END

SELECT * FROM Disc
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ Disc-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ DiscMusic-----------------------------------------
GO
BEGIN
	INSERT INTO DiscMusic
	SELECT
		Id, (ABS(CHECKSUM(NewId())) % (SELECT MAX(Id) FROM Music) +  1)
	FROM Disc
	WHERE Disc.Name LIKE '%Music%'
END
SELECT * FROM DiscMusic ORDER BY IdDisc
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ DiscMusic-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ DiscFilm-----------------------------------------
GO
BEGIN
	INSERT INTO DiscFilm
	SELECT
		Id, (ABS(CHECKSUM(NewId())) % (SELECT MAX(Id) FROM Film) +  1)
	FROM Disc
	WHERE Disc.Name LIKE '%Film%'
END
SELECT * FROM DiscFilm ORDER BY IdDisc
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ DiscFilm-----------------------------------------





-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ OperationLog-----------------------------------------
GO
------------------ГЕНЕРАЦІЯ РАНДОМНОЇ ДАТИ------------------
DECLARE @StartDate AS DATE;
DECLARE @EndDate AS DATE;

SELECT @StartDate = '01/01/2014',
       @EndDate = DATEADD(DAY, -60, GETDATE());
------------------ГЕНЕРАЦІЯ РАНДОМНОЇ ДАТИ------------------

BEGIN
	DECLARE @types TABLE (id int identity(1, 1), type varchar(10))
	insert into @types values('Покупка'), ('Оренда')

	DECLARE @number INT = 150000, @OperationType VARCHAR(10), @OperationStart DATETIME, @OperationEnd DATETIME, @IdClient INT, @IdDisc INT

	WHILE @number > 0
	BEGIN
		SET @OperationType = (SELECT type FROM @types WHERE id = FLOOR(RAND()*(2-1+1)+1))

		
		SET @OperationStart = DATEADD(DAY, RAND(CHECKSUM(NEWID()))*(1+DATEDIFF(DAY, @StartDate, @EndDate)),@StartDate)


		DECLARE @RandomDebtor INT = FLOOR(RAND()*(10-1+1)+1)
		SET @OperationEnd =	CASE
								WHEN @OperationType = 'Покупка'
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

		SET @IdClient = FLOOR(RAND()*((SELECT MAX(Id) FROM Client) - 1 + 1) + 1)
		SET @IdDisc = FLOOR(RAND()*((SELECT MAX(Id) FROM Disc) - 1 + 1) + 1)

		INSERT INTO OperationLog
		VALUES (@OperationType, @OperationStart, @OperationEnd, @IdClient, @IdDisc)

		SET @number -= 1
	END
END

SELECT *, DATEDIFF(DAY, OperationDateTimeStart, OperationDateTimeEnd) AS DATEDIFF FROM OperationLog order by IdClient
-----------------------------------------ГЕНЕРАЦІЯ ДАНИХ ДЛЯ ТАБЛИЦІ OperationLog-----------------------------------------