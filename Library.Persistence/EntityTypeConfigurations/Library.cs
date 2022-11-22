using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EntityTypeConfigurations;

public class Library : IEntityTypeConfiguration<Model.Library>
{
    public void Configure(EntityTypeBuilder<Model.Library> builder)
    {
        builder.HasKey(l => l.LibraryId);
        builder.Property(l => l.LibraryName).IsRequired();
        builder.HasIndex(l => l.LibraryName).IsUnique();
        builder.HasMany(u => u.Users)
            .WithOne(l => l.Library)
            .HasForeignKey("LibraryId").IsRequired();
        builder.HasMany(l => l.Books)
            .WithOne(b => b.Library);
    }
}