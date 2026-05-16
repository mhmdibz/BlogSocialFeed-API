namespace Blog.Application.DTOs;

public record CreateUserDto(
    string UserName,
    string Email,
    string? Bio
);

public record UpdateUserDto(
    string? UserName,
    string? Bio
);

public record UserDto(
    int Id,
    string UserName,
    string Email,
    string? Bio,
    DateTime CreatedAt,
    int PostsCount,
    int CommentsCount,
    int FollowersCount,
    int FollowingCount
);

public record UserListDto(
    int Id,
    string UserName,
    string Email,
    string? Bio
);
