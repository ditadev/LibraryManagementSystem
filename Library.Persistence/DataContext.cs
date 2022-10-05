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
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=LibraryService;UserId=postgres;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var library = modelBuilder.Entity<Model.Library>();
        var customer = modelBuilder.Entity<Customer>();
        var book = modelBuilder.Entity<Book>();

        library.HasKey(l => l.LibraryId);
        customer.HasKey(c => c.Username);
        book.HasKey(b => b.ISBN);

        book.HasOne(c => c.Customers)
            .WithMany(b => b.Books)
            .HasForeignKey("CustomerId").IsRequired();
        customer.HasOne(c => c.Library)
            .WithMany(l => l.Customers).HasForeignKey("LibraryId");

        customer.Ignore(x => x.Password);
    }
}