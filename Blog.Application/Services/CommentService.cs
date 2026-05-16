using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Services;

public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        if (!await _postRepository.GetTotalCountAsync(cancellationToken).ContinueWith(_ => true))
            throw new InvalidOperationException($"Post with ID {dto.PostId} not found.");

        var post = await _postRepository.GetByIdAsync(dto.PostId, cancellationToken)
            ?? throw new KeyNotFoundException($"Post with ID {dto.PostId} not found.");

        if (!await _userRepository.ExistsAsync(dto.UserId, cancellationToken))
            throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");

        var comment = new Comment
        {
            Content = dto.Content,
            PostId = dto.PostId,
            UserId = dto.UserId
        };

        await _commentRepository.AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        comment = await _commentRepository.GetByIdAsync(comment.Id, cancellationToken);
        return MapToDto(comment!);
    }

    public async Task<CommentDto?> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        return comment == null ? null : MapToDto(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByPostIdAsync(postId, cancellationToken);
        return comments.Select(MapToDto);
    }

    public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Comment with ID {id} not found.");

        comment.Content = dto.Content;
        _commentRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(comment);
    }

    public async Task DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Comment with ID {id} not found.");

        _commentRepository.Delete(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static CommentDto MapToDto(Comment comment) => new(
        comment.Id,
        comment.Content,
        comment.CreatedAt,
        comment.User == null ? null : new UserListDto(comment.User.Id, comment.User.UserName, comment.User.Email.Value, comment.User.Bio),
        comment.Reactions.Count
    );
}
