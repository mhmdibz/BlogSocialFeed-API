using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Bio).HasMaxLength(280);

            //builder.OwnsOne(u => u.Email, emailBuilder =>
            //{
            //    emailBuilder.Property(e => e.Value)
            //        .HasColumnName("Email")
            //        .HasMaxLength(200)
            //        .IsRequired();

            //    emailBuilder.WithOwner();
            //});

            builder.Property(u => u.Email)
                .IsRequired()
                .HasConversion(
                    email => email.Value,
                    value => (Email)value)
                .HasMaxLength(200);


            builder.Property(u => u.IsDeleted).HasDefaultValue(false);

            builder.Property(u => u.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.HasQueryFilter(u => !u.IsDeleted);

            // Configure navigation properties to use backing fields
            builder.HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Metadata.FindNavigation(nameof(User.Posts))!.SetField("_posts");

            builder.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Metadata.FindNavigation(nameof(User.Comments))!.SetField("_comments");

            builder.HasMany(u => u.Reactions)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Metadata.FindNavigation(nameof(User.Reactions))!.SetField("_reactions");

            builder.HasMany(u => u.Followers)
                .WithOne(f => f.Followee)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Metadata.FindNavigation(nameof(User.Followers))!.SetField("_followers");

            builder.HasMany(u => u.Following)
                .WithOne(f => f.Follower)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Metadata.FindNavigation(nameof(User.Following))!.SetField("_following");
        }
    }
}
