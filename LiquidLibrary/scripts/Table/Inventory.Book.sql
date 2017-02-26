--DROP SCHEMA Inventory

CREATE SCHEMA Inventory
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Book')
	DROP TABLE Inventory.Book
GO

CREATE TABLE Inventory.Book
(
	Id UNIQUEIDENTIFIER NOT NULL,
	CreatedOn DATETIMEOFFSET NULL,
	CreatedBy_Id UNIQUEIDENTIFIER NULL,
	LastModifiedOn DATETIMEOFFSET NULL,
	LastModifiedBy_Id UNIQUEIDENTIFIER NULL,
	Name NVARCHAR(255) NOT NULL,
	Author NVARCHAR(255) NOT NULL,
	ISBN NVARCHAR(255) NOT NULL
)

ALTER TABLE Inventory.Book
	ADD PRIMARY KEY (Id)