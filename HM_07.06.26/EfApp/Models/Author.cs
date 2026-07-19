namespace EfApp.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public List<Book> Books { get; set; } = new();
}
