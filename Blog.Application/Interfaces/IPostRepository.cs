using Blog.Domain.Entities;

namespace Blog.Application.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Post?> GetByIdWithCommentsAsync(int id, CancellationToken cancellationToken = default);
    Task<Post?> GetByIdWithReactionsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetByUserIdAsync(int userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetRecentPostsAsync(int count, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    void Update(Post post);
    void Delete(Post post);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
