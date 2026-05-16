namespace Blog.Application.DTOs;

public record CreatePostDto(
    int UserId,
    string Content
);

public record UpdatePostDto(
    string Content
);

public record PostDto(
    int Id,
    string Content,
    DateTime CreatedAt,
    UserListDto? User,
    int CommentsCount,
    int ReactionsCount
);

public record PostDetailDto(
    int Id,
    string Content,
    DateTime CreatedAt,
    UserListDto? User,
    List<CommentDto> Comments,
    Dictionary<string, int> ReactionCounts
);
