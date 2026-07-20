using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=products.db");
    }
}
