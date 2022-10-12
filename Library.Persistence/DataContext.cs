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

    public DbSet<Model.Library> Libraries { get; set; }
    public DbSet<User> Customers { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=LibraryService;UserId=postgres;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var library = modelBuilder.Entity<Model.Library>();
        var customer = modelBuilder.Entity<User>();
        var book = modelBuilder.Entity<Book>();
     
        
        library.HasKey(l => l.LibraryId);
        customer.HasKey(c => c.UserId);
        book.HasKey(b => b.ISBN);
       
        
        customer.HasOne(c => c.Library)
            .WithMany(l => l.Customers).HasForeignKey("LibraryId");

        customer.Ignore(x => x.Password);
       
    }
}