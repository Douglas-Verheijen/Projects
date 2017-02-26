
CREATE SCHEMA [Security]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'AuthenticationSettings')
	DROP TABLE [Security].AuthenticationSettings
GO

CREATE TABLE [Security].AuthenticationSettings
(
	Id UNIQUEIDENTIFIER NOT NULL,
	CreatedOn DATETIMEOFFSET NULL,
	CreatedBy_Id UNIQUEIDENTIFIER NULL,
	LastModifiedOn DATETIMEOFFSET NULL,
	LastModifiedBy_Id UNIQUEIDENTIFIER NULL,
	Mode NVARCHAR(255) NOT NULL,
)

ALTER TABLE [Security].AuthenticationSettings
	ADD PRIMARY KEY (Id)

INSERT INTO [Security].AuthenticationSettings
(
	Id,
	CreatedOn,
	LastModifiedOn,
	Mode
)
VALUES
(
	NEWID(),
	GETDATE(),
	GETDATE(),
	'Local'
)