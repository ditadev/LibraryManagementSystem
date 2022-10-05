using Microsoft.EntityFrameworkCore;
using Library.Model;
using Microsoft.Extensions.DependencyModel;

namespace Library.Persistence;
public class DataContext:DbContext
{
    
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=Library;UserId=postgres;");
    }
    public DbSet<Model.Library> Libraries { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Book> Books { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var library = modelBuilder.Entity<Model.Library>();
        var customer = modelBuilder.Entity<Customer>();
        var book = modelBuilder.Entity<Book>();

        library.HasKey(l => l.LibraryName);
        customer.HasKey(c => c.Username);
        book.HasKey(b => b.ISBN);
        library.HasMany(l => l.Books)
            .WithOne(b => b.Library)
            .HasForeignKey("LibraryName").IsRequired();
        book.HasOne(c => c.Customers)
            .WithMany(b => b.Books)
            .HasForeignKey("CustomerId").IsRequired();

        customer.Ignore(x => x.Password);
    }

}