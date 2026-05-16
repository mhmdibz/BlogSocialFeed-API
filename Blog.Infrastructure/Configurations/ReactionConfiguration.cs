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
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Kind)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Unique constraint: One reaction per user per post (user can change reaction kind)
            builder.HasIndex(r => new { r.PostId, r.UserId })
                .IsUnique()
                .HasFilter("[PostId] IS NOT NULL");

            // Unique constraint: One reaction per user per comment
            builder.HasIndex(r => new { r.CommentId, r.UserId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL");

            // Separate index for querying by kind
            builder.HasIndex(r => new { r.PostId, r.Kind })
                .HasFilter("[PostId] IS NOT NULL");

            builder.HasIndex(r => new { r.CommentId, r.Kind })
                .HasFilter("[CommentId] IS NOT NULL");

            builder.HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Comment)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
