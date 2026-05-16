using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Domain.Entities;

namespace Blog.Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(500);

       

        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.Property(c => c.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasQueryFilter(c => !c.IsDeleted);

        // Filtered index with descending order for better performance
        builder.HasIndex(c => new { c.PostId, c.CreatedAt })
            .IsDescending(false, true)
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
