using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant() && !u.IsDeleted, cancellationToken);

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted, cancellationToken);

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        => await _context.Users
            .Where(u => !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<User>> SearchByUserNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        => await _context.Users
            .Where(u => !u.IsDeleted && u.UserName.Contains(searchTerm))
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await _context.Users.AddAsync(user, cancellationToken);

    public void Update(User user)
        => _context.Users.Update(user);

    public void Delete(User user)
    {
        user.Delete(); // soft delete
        _context.Users.Update(user);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        => await _context.Users.CountAsync(u => !u.IsDeleted, cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
}
