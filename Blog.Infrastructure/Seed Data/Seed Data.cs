using Bogus;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Seed_Data
{
    public class Seed_Data
    {
        public static async Task SeedUsersAsync(BlogDbContext context, CancellationToken cancellationToken = default)
        {
            List<User>? users = null;
            List<Post>? posts = null;
            List<Comment>? comments = null;

            // Seed Users
            if (!await context.Users.AnyAsync(cancellationToken))
            {
                var userFaker = new Faker<User>()
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())
                    .RuleFor(u => u.Email, f => Email.Create(f.Internet.Email()))
                    .RuleFor(u => u.Bio, f => f.Lorem.Sentence(10));

                users = userFaker.Generate(50);
                await context.Users.AddRangeAsync(users, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Seed Posts
            if (!await context.Posts.AnyAsync(cancellationToken))
            {
                users ??= await context.Users.ToListAsync(cancellationToken);

                var postFaker = new Faker<Post>()
                    .RuleFor(p => p.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(p => p.Content, f => f.Lorem.Sentences(f.Random.Int(3, 10)));

                posts = postFaker.Generate(100);
                await context.Posts.AddRangeAsync(posts, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Seed Comments
            if (!await context.Comments.AnyAsync(cancellationToken))
            {
                users ??= await context.Users.ToListAsync(cancellationToken);
                posts ??= await context.Posts.ToListAsync(cancellationToken);

                var commentFaker = new Faker<Comment>()
                    .RuleFor(c => c.Content, f => f.Lorem.Sentences(f.Random.Int(1, 3)))
                    .RuleFor(c => c.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(c => c.PostId, f => f.PickRandom(posts).Id);

                comments = commentFaker.Generate(200);
                await context.Comments.AddRangeAsync(comments, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Seed Reactions
            if (!await context.Reactions.AnyAsync(cancellationToken))
            {
                users ??= await context.Users.ToListAsync(cancellationToken);
                posts ??= await context.Posts.ToListAsync(cancellationToken);
                comments ??= await context.Comments.ToListAsync(cancellationToken);

                var reactions = new List<Reaction>();

                // Post Reactions - avoiding duplicates
                var postReactionFaker = new Faker<Reaction>()
                    .RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(r => r.PostId, f => f.PickRandom(posts).Id)
                    .RuleFor(r => r.CommentId, f => null)
                    .RuleFor(r => r.Kind, f => f.PickRandom<ReactionKind>());

                var postReactions = postReactionFaker.Generate(150)
                    .GroupBy(r => new { r.UserId, r.PostId })
                    .Select(g => g.First())
                    .ToList();

                // Comment Reactions - avoiding duplicates
                var commentReactionFaker = new Faker<Reaction>()
                    .RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(r => r.PostId, f => null)
                    .RuleFor(r => r.CommentId, f => f.PickRandom(comments).Id)
                    .RuleFor(r => r.Kind, f => f.PickRandom<ReactionKind>());

                var commentReactions = commentReactionFaker.Generate(100)
                    .GroupBy(r => new { r.UserId, r.CommentId })
                    .Select(g => g.First())
                    .ToList();

                reactions.AddRange(postReactions);
                reactions.AddRange(commentReactions);

                await context.Reactions.AddRangeAsync(reactions, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
