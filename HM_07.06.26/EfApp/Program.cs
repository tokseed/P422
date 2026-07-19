using EfApp.Data;
using Microsoft.EntityFrameworkCore;
using EfApp.Models;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlite("Data Source=library.db");

using var db = new AppDbContext(optionsBuilder.Options);

db.Database.EnsureCreated();

if (!db.Authors.Any())
{
    var author1 = new Author { Name = "Лев Толстой", BirthYear = 1828 };
    var author2 = new Author { Name = "Фёдор Достоевский", BirthYear = 1821 };

    db.Authors.AddRange(author1, author2);
    db.Books.AddRange(
        new Book { Title = "Война и мир", Year = 1869, Author = author1 },
        new Book { Title = "Анна Каренина", Year = 1877, Author = author1 },
        new Book { Title = "Преступление и наказание", Year = 1866, Author = author2 },
        new Book { Title = "Идиот", Year = 1869, Author = author2 }
    );
    db.SaveChanges();
}

var query = db.Books
    .Include(b => b.Author)
    .OrderBy(b => b.Year)
    .Select(b => new
    {
        b.Title,
        b.Year,
        Author = b.Author != null ? $"{b.Author.Name} ({b.Author.BirthYear})" : "Неизвестен"
    });

foreach (var item in query)
{
    Console.WriteLine($"{item.Title} — {item.Year}, автор: {item.Author}");
}
