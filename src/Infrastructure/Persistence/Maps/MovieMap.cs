using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps;

public class MovieMap : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Title).IsUnique();

        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(500);

        builder.HasMany(x => x.Ratings).WithMany(x => x.Movies);
    }
}
