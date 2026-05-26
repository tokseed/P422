IF DB_ID('Shop') IS NULL
BEGIN
    CREATE DATABASE Shop;
END
GO

USE Shop;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        City NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    INSERT INTO dbo.Users (FirstName, LastName, Email, City, CreatedAt)
    VALUES
        (N'Ivan', N'Petrov', N'ivan.petrov@example.com', N'Almaty', SYSDATETIME()),
        (N'Anna', N'Sidorova', N'anna.sidorova@example.com', N'Astana', DATEADD(DAY, -1, SYSDATETIME())),
        (N'Maksim', N'Orlov', N'maksim.orlov@example.com', N'Karaganda', DATEADD(DAY, -2, SYSDATETIME()));
END
GO
