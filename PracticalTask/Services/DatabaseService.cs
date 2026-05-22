using System.Data;
using Microsoft.Data.SqlClient;
using PracticalTask.Models;

namespace PracticalTask.Services;

public class DatabaseService
{
    private const string ServerConnectionString =
        "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrongPassword123;TrustServerCertificate=True;Connect Timeout=30;Encrypt=True;";

    private const string ConnectionString =
        "Server=localhost,1433;Database=BooksDb;User Id=sa;Password=YourStrongPassword123;TrustServerCertificate=True;Connect Timeout=30;Encrypt=True;";

    public DatabaseService()
    {
        EnsureDatabase();
    }

    public List<Book> GetBooks()
    {
        var books = new List<Book>();

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand("SELECT Id, Title, Author, PublishYear, Genre FROM Books ORDER BY Id", connection);

        connection.Open();
        using var reader = command.ExecuteReader();

        var idIndex = reader.GetOrdinal("Id");
        var titleIndex = reader.GetOrdinal("Title");
        var authorIndex = reader.GetOrdinal("Author");
        var publishYearIndex = reader.GetOrdinal("PublishYear");
        var genreIndex = reader.GetOrdinal("Genre");

        while (reader.Read())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(idIndex),
                Title = reader.GetString(titleIndex),
                Author = reader.GetString(authorIndex),
                PublishYear = reader.GetInt32(publishYearIndex),
                Genre = reader.GetString(genreIndex)
            });
        }

        return books;
    }

    public void AddBook(Book book)
    {
        const string sql = """
                           INSERT INTO Books (Title, Author, PublishYear, Genre)
                           VALUES (@Title, @Author, @PublishYear, @Genre)
                           """;

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        FillParameters(command, book);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void UpdateBook(Book book)
    {
        const string sql = """
                           UPDATE Books
                           SET Title = @Title,
                               Author = @Author,
                               PublishYear = @PublishYear,
                               Genre = @Genre
                           WHERE Id = @Id
                           """;

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        FillParameters(command, book);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void DeleteBook(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand("DELETE FROM Books WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        connection.Open();
        command.ExecuteNonQuery();
    }

    private static void FillParameters(SqlCommand command, Book book)
    {
        command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = book.Title;
        command.Parameters.Add("@Author", SqlDbType.NVarChar).Value = book.Author;
        command.Parameters.Add("@PublishYear", SqlDbType.Int).Value = book.PublishYear;
        command.Parameters.Add("@Genre", SqlDbType.NVarChar).Value = book.Genre;
    }

    private void EnsureDatabase()
    {
        const string createDatabaseSql = """
                                         IF DB_ID('BooksDb') IS NULL
                                             CREATE DATABASE BooksDb;
                                         """;

        const string createTableSql = """
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
    }
}
