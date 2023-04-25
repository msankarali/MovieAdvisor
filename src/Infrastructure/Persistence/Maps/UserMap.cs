using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Email).HasMaxLength(500).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(300).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(300).IsRequired();

        builder.HasMany(x => x.Ratings).WithOne(x => x.User).HasForeignKey(x => x.UserId);
    }
}
