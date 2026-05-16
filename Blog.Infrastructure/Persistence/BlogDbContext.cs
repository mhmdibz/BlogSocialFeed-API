using Blog.Domain.Entities;
using Blog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Persistence
{
    public class BlogDbContext : DbContext
    {
        const string ConnectionString = "Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Reaction> Reactions => Set<Reaction>();
        public DbSet<Follow> Follows => Set<Follow>();

        // ✅ للـ API — Dependency Injection بياخد الـ options من الـ appsettings
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        // ✅ للـ Console — بيشتغل بدون options ويستخدم الـ ConnectionString الـ hardcoded
        public BlogDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = utcNow;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = utcNow;
                        break;
                }
            }
        }

        #region SaveChanges Overrides
        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        #endregion

        #region Async SaveChanges Overrides
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}