namespace Blog.Application.DTOs;

public record CreateReactionDto(
    int UserId,
    int? PostId,
    int? CommentId,
    string Kind // "Like", "Love", "Clap", "Smile"
);

public record UpdateReactionDto(
    string Kind
);

public record ReactionDto(
    int Id,
    string Kind,
    UserListDto? User
);
