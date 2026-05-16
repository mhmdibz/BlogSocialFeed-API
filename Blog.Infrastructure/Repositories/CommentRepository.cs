using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Reactions)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

    public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await _context.Comments
            .Where(c => c.PostId == postId && !c.IsDeleted)
            .Include(c => c.User)
            .Include(c => c.Reactions)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        => await _context.Comments
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .Include(c => c.Post)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
        => await _context.Comments.AddAsync(comment, cancellationToken);

    public void Update(Comment comment)
        => _context.Comments.Update(comment);

    public void Delete(Comment comment)
    {
        comment.Delete();
        _context.Comments.Update(comment);
    }

    public async Task<int> GetCountByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await _context.Comments.CountAsync(c => c.PostId == postId && !c.IsDeleted, cancellationToken);
}
