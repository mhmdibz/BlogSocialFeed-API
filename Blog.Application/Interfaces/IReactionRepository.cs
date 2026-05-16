using Blog.Domain.Entities;
using Blog.Domain.Enums;

namespace Blog.Application.Interfaces;

public interface IReactionRepository
{
    Task<Reaction?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Reaction?> GetUserReactionForPostAsync(int userId, int postId, CancellationToken cancellationToken = default);
    Task<Reaction?> GetUserReactionForCommentAsync(int userId, int commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reaction>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reaction>> GetByCommentIdAsync(int commentId, CancellationToken cancellationToken = default);
    Task<Dictionary<ReactionKind, int>> GetReactionCountsByPostIdAsync(int postId, CancellationToken cancellationToken = default);
    Task AddAsync(Reaction reaction, CancellationToken cancellationToken = default);
    void Update(Reaction reaction);
    void Delete(Reaction reaction);
}
