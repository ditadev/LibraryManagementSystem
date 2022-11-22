using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EntityTypeConfigurations;

public class Book : IEntityTypeConfiguration<Model.Book>
{
    public void Configure(EntityTypeBuilder<Model.Book> builder)
    {
        builder.HasKey(b => b.ISBN);
        builder.HasIndex(b => b.ISBN).IsUnique();
        builder.Property(b => b.Title).IsRequired();
        builder.Property(b => b.Genre).IsRequired();
        builder.Property(b => b.Available).IsRequired();
        builder.Property(b => b.Author).IsRequired();
        builder.Property(b => b.LibraryId).IsRequired();
        builder.Property(b => b.Collected).IsRequired();
        builder.Property(b => b.ReturnDate).IsRequired();
    }
}