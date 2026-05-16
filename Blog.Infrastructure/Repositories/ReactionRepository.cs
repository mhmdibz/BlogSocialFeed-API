using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class ReactionRepository : IReactionRepository
{
    private readonly BlogDbContext _context;

    public ReactionRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Reaction?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);

    public async Task<Reaction?> GetUserReactionForPostAsync(int userId, int postId, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId && !r.IsDeleted, cancellationToken);

    public async Task<Reaction?> GetUserReactionForCommentAsync(int userId, int commentId, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.CommentId == commentId && !r.IsDeleted, cancellationToken);

    public async Task<IEnumerable<Reaction>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .Where(r => r.PostId == postId && !r.IsDeleted)
            .Include(r => r.User)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Reaction>> GetByCommentIdAsync(int commentId, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .Where(r => r.CommentId == commentId && !r.IsDeleted)
            .Include(r => r.User)
            .ToListAsync(cancellationToken);

    public async Task<Dictionary<ReactionKind, int>> GetReactionCountsByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await _context.Reactions
            .Where(r => r.PostId == postId && !r.IsDeleted)
            .GroupBy(r => r.Kind)
            .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);

    public async Task AddAsync(Reaction reaction, CancellationToken cancellationToken = default)
        => await _context.Reactions.AddAsync(reaction, cancellationToken);

    public void Update(Reaction reaction)
        => _context.Reactions.Update(reaction);

    public void Delete(Reaction reaction)
    {
        reaction.Delete();
        _context.Reactions.Update(reaction);
    }
}
