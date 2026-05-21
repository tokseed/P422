IF DB_ID('BooksDb') IS NULL
BEGIN
    CREATE DATABASE BooksDb;
END
GO

USE BooksDb;
GO

IF OBJECT_ID('dbo.Books', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(100) NOT NULL,
        Author NVARCHAR(100) NOT NULL,
        PublishYear INT NOT NULL,
        Genre NVARCHAR(50) NOT NULL
    );
END
GO
