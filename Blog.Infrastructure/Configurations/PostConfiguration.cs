using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.IsDeleted).HasDefaultValue(false);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.HasQueryFilter(p => !p.IsDeleted);

            // Covering index for better query performance
            builder.HasIndex(p => new { p.UserId, p.CreatedAt })
                .IsDescending(false, true)
                .IncludeProperties(p => p.Content);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
