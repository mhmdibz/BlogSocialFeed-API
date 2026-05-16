using Blog.Application.Interfaces;
using Blog.Infrastructure.Persistence;

namespace Blog.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDbContext _context;

    public UnitOfWork(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public int SaveChanges()
        => _context.SaveChanges();
}
