using EfAppCrud.Data;
using EfAppCrud.Models;
using Microsoft.EntityFrameworkCore;

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

while (true)
{
    Console.WriteLine("\n=== Библиотека (EF CRUD) ===");
    Console.WriteLine("1. Показать всех авторов и книги");
    Console.WriteLine("2. Добавить автора");
    Console.WriteLine("3. Добавить книгу");
    Console.WriteLine("4. Редактировать автора");
    Console.WriteLine("5. Редактировать книгу");
    Console.WriteLine("6. Удалить автора");
    Console.WriteLine("7. Удалить книгу");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1": ShowAll(db); break;
        case "2": AddAuthor(db); break;
        case "3": AddBook(db); break;
        case "4": EditAuthor(db); break;
        case "5": EditBook(db); break;
        case "6": DeleteAuthor(db); break;
        case "7": DeleteBook(db); break;
        case "0": return;
        default: Console.WriteLine("Неверный выбор."); break;
    }
}

static void ShowAll(AppDbContext db)
{
    var authors = db.Authors.Include(a => a.Books).OrderBy(a => a.Name).ToList();
    foreach (var a in authors)
    {
        Console.WriteLine($"\nАвтор #{a.Id}: {a.Name} ({a.BirthYear})");
        if (a.Books.Count == 0)
            Console.WriteLine("  Нет книг");
        foreach (var b in a.Books)
            Console.WriteLine($"  - #{b.Id} {b.Title} ({b.Year})");
    }
}

static void AddAuthor(AppDbContext db)
{
    Console.Write("Имя автора: ");
    var name = Console.ReadLine() ?? string.Empty;
    Console.Write("Год рождения: ");
    if (!int.TryParse(Console.ReadLine(), out var year))
    {
        Console.WriteLine("Неверный год.");
        return;
    }

    db.Authors.Add(new Author { Name = name, BirthYear = year });
    db.SaveChanges();
    Console.WriteLine("Автор добавлен.");
}

static void AddBook(AppDbContext db)
{
    var authors = db.Authors.OrderBy(a => a.Name).ToList();
    if (authors.Count == 0)
    {
        Console.WriteLine("Сначала добавьте автора.");
        return;
    }

    Console.WriteLine("Доступные авторы:");
    foreach (var a in authors)
        Console.WriteLine($"  #{a.Id} {a.Name}");

    Console.Write("ID автора: ");
    if (!int.TryParse(Console.ReadLine(), out var authorId) || authors.All(a => a.Id != authorId))
    {
        Console.WriteLine("Неверный ID автора.");
        return;
    }

    Console.Write("Название книги: ");
    var title = Console.ReadLine() ?? string.Empty;
    Console.Write("Год издания: ");
    if (!int.TryParse(Console.ReadLine(), out var year))
    {
        Console.WriteLine("Неверный год.");
        return;
    }

    db.Books.Add(new Book { Title = title, Year = year, AuthorId = authorId });
    db.SaveChanges();
    Console.WriteLine("Книга добавлена.");
}

static void EditAuthor(AppDbContext db)
{
    Console.Write("ID автора для редактирования: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Неверный ID.");
        return;
    }

    var author = db.Authors.FirstOrDefault(a => a.Id == id);
    if (author == null)
    {
        Console.WriteLine("Автор не найден.");
        return;
    }

    Console.Write($"Новое имя (сейчас: {author.Name}): ");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name)) author.Name = name;

    Console.Write($"Новый год рождения (сейчас: {author.BirthYear}): ");
    var yearInput = Console.ReadLine();
    if (int.TryParse(yearInput, out var year)) author.BirthYear = year;

    db.SaveChanges();
    Console.WriteLine("Автор обновлён.");
}

static void EditBook(AppDbContext db)
{
    Console.Write("ID книги для редактирования: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Неверный ID.");
        return;
    }

    var book = db.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        Console.WriteLine("Книга не найдена.");
        return;
    }

    Console.Write($"Новое название (сейчас: {book.Title}): ");
    var title = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(title)) book.Title = title;

    Console.Write($"Новый год (сейчас: {book.Year}): ");
    var yearInput = Console.ReadLine();
    if (int.TryParse(yearInput, out var year)) book.Year = year;

    Console.Write($"Новый ID автора (сейчас: {book.AuthorId}): ");
    var authorInput = Console.ReadLine();
    if (int.TryParse(authorInput, out var authorId)) book.AuthorId = authorId;

    db.SaveChanges();
    Console.WriteLine("Книга обновлена.");
}

static void DeleteAuthor(AppDbContext db)
{
    Console.Write("ID автора для удаления: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Неверный ID.");
        return;
    }

    var author = db.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);
    if (author == null)
    {
        Console.WriteLine("Автор не найден.");
        return;
    }

    db.Books.RemoveRange(author.Books);
    db.Authors.Remove(author);
    db.SaveChanges();
    Console.WriteLine("Автор и его книги удалены.");
}

static void DeleteBook(AppDbContext db)
{
    Console.Write("ID книги для удаления: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Неверный ID.");
        return;
    }

    var book = db.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        Console.WriteLine("Книга не найдена.");
        return;
    }

    db.Books.Remove(book);
    db.SaveChanges();
    Console.WriteLine("Книга удалена.");
}
