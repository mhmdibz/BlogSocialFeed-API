using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _context;

    public PostRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);

    public async Task<Post?> GetByIdWithCommentsAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments).ThenInclude(c => c.User)
            .Include(p => p.Comments).ThenInclude(c => c.Reactions)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);

    public async Task<Post?> GetByIdWithReactionsAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Reactions).ThenInclude(r => r.User)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);

    public async Task<IEnumerable<Post>> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Where(p => !p.IsDeleted)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Post>> GetByUserIdAsync(int userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Post>> GetRecentPostsAsync(int count, CancellationToken cancellationToken = default)
        => await _context.Posts
            .Where(p => !p.IsDeleted)
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
        => await _context.Posts.AddAsync(post, cancellationToken);

    public void Update(Post post)
        => _context.Posts.Update(post);

    public void Delete(Post post)
    {
        post.Delete();
        _context.Posts.Update(post);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        => await _context.Posts.CountAsync(p => !p.IsDeleted, cancellationToken);

    public async Task<int> GetCountByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        => await _context.Posts.CountAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);
}
