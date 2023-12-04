Database scheme:
================

![DatabaseScheme](/СdDiskStoreAspNetCore/wwwroot/images/TablesScheme.png)

  

### Table: Client

	Stores information about clients.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the Client table, not null, default NEWID() |
| FirstName | NVARCHAR(50) | No  | No  | First name of the client, not null |
| LastName | NVARCHAR(50) | No  | No  | Last name of the client, not null |
| Address | NVARCHAR(100) | No  | No  | Address of the client, not null |
| City | NVARCHAR(50) | No  | No  | City of the client, not null |
| ContactPhone | NVARCHAR(20) | No  | No  | Contact phone number of the client, not null |
| ContactMail | NVARCHAR(100) | No  | No  | Contact email of the client, not null |
| BirthDay | DATE | No  | No  | Birth date of the client, not null |
| MarriedStatus | BIT | No  | No  | Marital status of the client (0 - Not Married, 1 - Married) |
| Sex | BIT | No  | No  | Gender of the client (0 - Female, 1 - Male) |
| HasChild | BIT | No  | No  | Whether the client has children |

### Table: PhoneList

	Stores phone numbers associated with clients.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the PhoneList table, not null, default NEWID() |
| Phone | NVARCHAR(20) | No  | Yes, references Client(Id) on delete cascade | Phone number, not null |
| IdClient | UNIQUEIDENTIFIER | No  | Yes, references Client(Id) on delete cascade | Foreign key referencing Client table, not null |

### Table: MailList

	Stores additional email addresses associated with clients.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the MailList table, not null, default NEWID() |
| Mail | NVARCHAR(100) | No  | Yes, references Client(Id) on delete cascade | Email address, not null |
| IdClient | UNIQUEIDENTIFIER | No  | Yes, references Client(Id) on delete cascade | Foreign key referencing Client table, not null |

### Table: PersonalDiscount

	Stores personal discounts associated with clients.
	Personal client's discount. 0 in default. As the profit from the client reaches out
	5000 UAH PersonalDiscount will get 5%.  
	15000 UAH - 10%,  
	30000 UAH - 15%,  
	50000 UAH - 20%.

When the client has a debt the discount doean't work out. In case when the debt hasn't been repayed the client goes down one level of the discounts. Rent: 1 month. The rent price is 50% of purhcase price

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the PersonalDiscount table, not null, default NEWID() |
| IdClient | UNIQUEIDENTIFIER | No  | Yes, references Client(Id) on delete cascade | Foreign key referencing Client table, not null |
| StartDateTime | DATETIME | No  | No  | Not null, start date and time for the personal discount |
| EndDateTime | DATETIME | No  | No  | End date and time for the personal discount |
| PersonalDiscountValue | INT | No  | No  | Not null, default 0, value of the personal discount |

### Table: Music

	Stores information about music.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the Music table, not null, default NEWID() |
| Name | NVARCHAR(50) | No  | No  | Not null, name of the music |
| Genre | NVARCHAR(50) | No  | No  | Not null, genre of the music |
| Artist | NVARCHAR(50) | No  | No  | Not null, artist of the music |
| Language | NVARCHAR(50) | No  | No  | Not null, language of the music |

### Table: Disc

	Stores information about discs.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key for the Disc table, not null, default NEWID() |
| Name | NVARCHAR(50) | No  | No  | Not null, name of the disc |
| Price | DECIMAL(9, 2) | No  | No  | Not null, price of the disc |

### Table: Film

	Stores information about films.

| Field Name | Data Type | Primary Key | Foreign Key | Check Constraint | Description |
| --- | --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | No  | Primary key for the Film table, not null, default NEWID() |
| Name | NVARCHAR(50) | No  | No  | No  | Not null, name of the film |
| Genre | NVARCHAR(50) | No  | No  | No  | Not null, genre of the film |
| Producer | NVARCHAR(50) | No  | No  | No  | Not null, producer of the film |
| MainRole | NVARCHAR(50) | No  | No  | No  | Not null, main role in the film |
| AgeLimit | INT | No  | No  | AgeLimit >= 0 | Not null, age limit for the film |

### Table: DiscMusic

	Stores information about discs with music.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| IdDisc | UNIQUEIDENTIFIER | No  | Yes, references Disc(Id) on delete cascade | Foreign key referencing Disc table |
| IdMusic | UNIQUEIDENTIFIER | No  | Yes, references Music(Id) on delete cascade | Foreign key referencing Music table |

### Table: DiscFilm

	Stores information about discs with film.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| IdDisc | UNIQUEIDENTIFIER | No  | Yes, references Disc(Id) on delete cascade | Foreign key referencing Disc table |
| IdFilm | UNIQUEIDENTIFIER | No  | Yes, references Film(Id) on delete cascade | Foreign key referencing Film table |

### Table: OperationType

	Stores information about types of operation client can make (Purchase or Rent).

| Field Name | Data Type | Primary Key | Description |
| --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | Primary key with default value NEWID() |
| TypeName | NVARCHAR(30) | No  | Name of the operation type |

### Table: OperationLog

	Stores log information about all operations.

| Field Name | Data Type | Primary Key | Foreign Key | Description |
| --- | --- | --- | --- | --- |
| Id  | UNIQUEIDENTIFIER | Yes | No  | Primary key with default value NEWID() |
| OperationType | UNIQUEIDENTIFIER | No  | Yes (References OperationType(Id) on delete cascade) | Operation type associated with the log entry |
| OperationDateTimeStart | DATETIME | No  | No  | Start date and time of the operation (since 2014) |
| OperationDateTimeEnd | DATETIME | No  | No  | End date and time of the operation. In the case of purchase or rent when the disc wasn't returned the value is NULL |
| IdClient | UNIQUEIDENTIFIER | No  | Yes (References Client(Id) on delete cascade) | Client associated with the operation |
| IdDisc | UNIQUEIDENTIFIER | No  | Yes (References Disc(Id) on delete cascade) | Disc associated with the operation |

* * *

Roless acced levels
===================

### Access levels:

	6 - CRUD + Admin Panel + Stored Procedures call + Views call
	5 – CRUD
	4 – C
	3 – D
	2 – U
	1 – R
	0 – No Access

| Tables/Roles | Admin | Manager | User | Guest |
| --- | --- | --- | --- | --- |
| Client | 6   | 5   | 1   | 0   |
| PhoneList | 6   | 5   | 1   | 0   |
| MailList | 6   | 5   | 1   | 0   |
| PersonalDiscount | 6   | 5   | 1   | 0   |
| Music | 6   | 5   | 1   | 1   |
| Disc | 6   | 5   | 1   | 1   |
| Film | 6   | 5   | 1   | 1   |
| DiscMusic | 6   | 5   | 1   | 1   |
| DiscFilm | 6   | 5   | 1   | 1   |
| OperationType | 6   | 5   | 1   | 0   |
| OperationLog | 6   | 5   | 1   | 0   |

* * *

Files
=====

### TablesCreation.sql

	This SQL file is responsible for creating the necessary database tables for the CD Disk Store application. It includes the creation of tables such as Client, PhoneList, MailList, PersonalDiscount, Music, Disc, Film, DiscMusic, DiscFilm, OperationType, and OperationLog.
	The script also drops the tables if they exist before creating them, ensuring a fresh start.

### DataGeneration.sql

	This file contains SQL queries for generating random data related to clients, phones, emails, music, films, discs, and operations.

### Backup.sql

	Contains script of backing up the database

Triggers
--------

### OperationLogInsertUpdate.sql

	Contains script of creating the trigger for OperationLog table after the INSERT and UPDATE command which should add the new data to PersonalDiscount table.

Views
-----

### Debtors.sql

	The list of debtors and the period of indebtedness.

### AdultFilmRatioByMonth.sql

	The percentage of adult films rented by male ('M') and female ('F') clients for each month of the year. The calculation is based on the total number of film rental operations and the count of adult film rentals.

### IncomePerEachFinancialQuarter.sql

	Income for each quarter of each year.

### ProfitFromEachClientStatistics.sql

	Profit from each customer from renting and buying discs with music and films in the format: [Client's Id] [OrderYear] [MusicPurchase], [MusicRent], [FilmPurchase], [FilmRent].

### UselessDiscs.sql

	The discs which haven't been bought or rented by anyone.

Stored Procedures
-----------------

### ChangeDiscountLevelForClient.sql

	Change the level of discount for a certain client.

### ChangeDiscTypePrice.sql

	Inclines the prices for each disc in a certain category.

Functions
---------

### GetTotalProfitFromClient.sql

	Calculation of the total income received from the client.

### GetTotalPurchaseProfitFromClient.sql

	Calculation of income received from a customer for purchases of disks.

### GetTotalRentProfitFromClient.sql

	Calculation of income received from a customer for renting disks.