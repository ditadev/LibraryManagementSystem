using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EntityTypeConfigurations;

public class User : IEntityTypeConfiguration<Model.User>
{
    public void Configure(EntityTypeBuilder<Model.User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.Firstname).IsRequired();
        builder.Property(u => u.Lastname).IsRequired();
        builder.Property(u => u.Email).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Address).IsUnique();
        builder.Property(u => u.Roles).IsRequired();
        builder.Ignore(u => u.Password);
    }
}