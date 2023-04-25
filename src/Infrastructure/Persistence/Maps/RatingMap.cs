using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps;

public class RatingMap : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.Score).IsRequired();

        builder.HasOne(x => x.Movie).WithMany(x => x.Ratings).HasForeignKey(x => x.MovieId);
    }
}
