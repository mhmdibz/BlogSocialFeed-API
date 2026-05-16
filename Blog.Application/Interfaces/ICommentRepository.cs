using Blog.Domain.Entities;

namespace Blog.Application.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    void Update(Comment comment);
    void Delete(Comment comment);
    Task<int> GetCountByPostIdAsync(int postId, CancellationToken cancellationToken = default);
}
