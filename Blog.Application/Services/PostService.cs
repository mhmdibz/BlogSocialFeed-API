using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Services;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IPostRepository postRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PostDto> CreatePostAsync(CreatePostDto dto, CancellationToken cancellationToken = default)
    {
        // Validate user exists
        if (!await _userRepository.ExistsAsync(dto.UserId, cancellationToken))
        {
            throw new InvalidOperationException($"User with ID {dto.UserId} not found.");
        }

        var post = new Post
        {
            UserId = dto.UserId,
            Content = dto.Content
        };

        await _postRepository.AddAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with user
        post = await _postRepository.GetByIdAsync(post.Id, cancellationToken);
        return MapToDto(post!);
    }

    public async Task<PostDto?> GetPostByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        return post == null ? null : MapToDto(post);
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);
        return posts.Select(MapToDto);
    }

    public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(int userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetByUserIdAsync(userId, pageNumber, pageSize, cancellationToken);
        return posts.Select(MapToDto);
    }

    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto dto, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        post.Content = dto.Content;

        _postRepository.Update(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(post);
    }

    public async Task DeletePostAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        _postRepository.Delete(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    // Mapping
    private static PostDto MapToDto(Post post)
    {
        return new PostDto(
            post.Id,
            post.Content,
            post.CreatedAt,
            post.User == null ? null : new UserListDto(post.User.Id, post.User.UserName, post.User.Email.Value, post.User.Bio),
            post.Comments.Count,
            post.Reactions.Count
        );
    }
}
