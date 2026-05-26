using System.Data;
using Microsoft.Data.SqlClient;
using HomeWork8.Models;

namespace HomeWork8.Services;

public class DatabaseService
{
    private const string ServerConnectionString =
        "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrongPassword123;TrustServerCertificate=True;Connect Timeout=30;Encrypt=True;";

    private const string ConnectionString =
        "Server=localhost,1433;Database=Shop;User Id=sa;Password=YourStrongPassword123;TrustServerCertificate=True;Connect Timeout=30;Encrypt=True;";

    public DatabaseService()
    {
        EnsureDatabase();
    }

    public List<User> GetUsers()
    {
        var users = new List<User>();

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(
            """
            SELECT Id, FirstName, LastName, Email, City, CreatedAt
            FROM Users
            ORDER BY Id
            """,
            connection);

        connection.Open();
        using var reader = command.ExecuteReader();

        var idIndex = reader.GetOrdinal("Id");
        var firstNameIndex = reader.GetOrdinal("FirstName");
        var lastNameIndex = reader.GetOrdinal("LastName");
        var emailIndex = reader.GetOrdinal("Email");
        var cityIndex = reader.GetOrdinal("City");
        var createdAtIndex = reader.GetOrdinal("CreatedAt");

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(idIndex),
                FirstName = reader.GetString(firstNameIndex),
                LastName = reader.GetString(lastNameIndex),
                Email = reader.GetString(emailIndex),
                City = reader.GetString(cityIndex),
                CreatedAt = reader.GetDateTime(createdAtIndex)
            });
        }

        return users;
    }

    private void EnsureDatabase()
    {
        const string createDatabaseSql = """
                                         IF DB_ID('Shop') IS NULL
                                             CREATE DATABASE Shop;
                                         """;

        const string createTableSql = """
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
                                      """;

        const string seedUsersSql = """
                                    IF NOT EXISTS (SELECT 1 FROM dbo.Users)
                                    BEGIN
                                        INSERT INTO dbo.Users (FirstName, LastName, Email, City, CreatedAt)
                                        VALUES
                                            (N'Ivan', N'Petrov', N'ivan.petrov@example.com', N'Almaty', SYSDATETIME()),
                                            (N'Anna', N'Sidorova', N'anna.sidorova@example.com', N'Astana', DATEADD(DAY, -1, SYSDATETIME())),
                                            (N'Maksim', N'Orlov', N'maksim.orlov@example.com', N'Karaganda', DATEADD(DAY, -2, SYSDATETIME()));
                                    END
                                    """;

        using (var connection = new SqlConnection(ServerConnectionString))
        using (var command = new SqlCommand(createDatabaseSql, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
        }

        using (var connection = new SqlConnection(ConnectionString))
        using (var command = new SqlCommand(createTableSql, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
        }

        using (var connection = new SqlConnection(ConnectionString))
        using (var command = new SqlCommand(seedUsersSql, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
