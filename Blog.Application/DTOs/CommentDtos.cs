namespace Blog.Application.DTOs;

public record CreateCommentDto(
    int PostId,
    int UserId,
    string Content
);

public record UpdateCommentDto(
    string Content
);

public record CommentDto(
    int Id,
    string Content,
    DateTime CreatedAt,
    UserListDto? User,
    int ReactionsCount
);
