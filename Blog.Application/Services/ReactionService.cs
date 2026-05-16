using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Enums;

namespace Blog.Application.Services;

public class ReactionService
{
    private readonly IReactionRepository _reactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactionService(
        IReactionRepository reactionRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _reactionRepository = reactionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReactionDto> ReactAsync(CreateReactionDto dto, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(dto.UserId, cancellationToken))
            throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");

        if (dto.PostId == null && dto.CommentId == null)
            throw new InvalidOperationException("Either PostId or CommentId must be provided.");

        if (!Enum.TryParse<ReactionKind>(dto.Kind, ignoreCase: true, out var kind))
            throw new ArgumentException($"Invalid reaction kind '{dto.Kind}'. Valid values: Like, Love, Clap, Smile.");

        // Check if user already reacted to this post/comment
        Reaction? existing = dto.PostId.HasValue
            ? await _reactionRepository.GetUserReactionForPostAsync(dto.UserId, dto.PostId.Value, cancellationToken)
            : await _reactionRepository.GetUserReactionForCommentAsync(dto.UserId, dto.CommentId!.Value, cancellationToken);

        if (existing != null)
        {
            // Update existing reaction
            existing.Kind = kind;
            _reactionRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MapToDto(existing);
        }

        var reaction = new Reaction
        {
            UserId = dto.UserId,
            PostId = dto.PostId,
            CommentId = dto.CommentId,
            Kind = kind
        };

        await _reactionRepository.AddAsync(reaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        reaction = await _reactionRepository.GetByIdAsync(reaction.Id, cancellationToken);
        return MapToDto(reaction!);
    }

    public async Task RemoveReactionAsync(int id, CancellationToken cancellationToken = default)
    {
        var reaction = await _reactionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Reaction with ID {id} not found.");

        _reactionRepository.Delete(reaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReactionDto>> GetReactionsByPostIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        var reactions = await _reactionRepository.GetByPostIdAsync(postId, cancellationToken);
        return reactions.Select(MapToDto);
    }

    public async Task<Dictionary<string, int>> GetReactionCountsByPostIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        var counts = await _reactionRepository.GetReactionCountsByPostIdAsync(postId, cancellationToken);
        return counts.ToDictionary(k => k.Key.ToString(), v => v.Value);
    }

    private static ReactionDto MapToDto(Reaction r) => new(
        r.Id,
        r.Kind.ToString(),
        r.User == null ? null : new UserListDto(r.User.Id, r.User.UserName, r.User.Email.Value, r.User.Bio)
    );
}
