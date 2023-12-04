USE [aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30]

GO
DECLARE @fileName VARCHAR(100), @dataBaseName VARCHAR(100), @fileDate VARCHAR(20);
SET @fileName = 'D:\';
SET @dataBaseName = 'aspnet-CdDiskStoreAspNetCore-c1b93b76-cb8b-480a-a3c4-45be8bbcad30';
SET @fileDate = CONVERT(VARCHAR(20), GETDATE(), 112);
SET @fileName = @fileName + @dataBaseName + '-' + @fileDate + '.bak';
BACKUP DATABASE @dataBaseName TO DISK = @fileName;