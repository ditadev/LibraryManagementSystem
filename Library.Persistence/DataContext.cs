using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Model.Library?> Libraries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.User());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.Book());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.Library());
    }
}

