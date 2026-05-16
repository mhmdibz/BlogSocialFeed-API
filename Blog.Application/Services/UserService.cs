using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        // Validate
        if (await _userRepository.GetByEmailAsync(dto.Email, cancellationToken) != null)
        {
            throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
        }

        if (await _userRepository.GetByUserNameAsync(dto.UserName, cancellationToken) != null)
        {
            throw new InvalidOperationException($"User with username '{dto.UserName}' already exists.");
        }

        // Create entity
        var user = new User
        {
            UserName = dto.UserName,
            Email = Email.Create(dto.Email),
            Bio = dto.Bio
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return user == null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<UserListDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);
        return users.Select(MapToListDto);
    }

    public async Task<IEnumerable<UserListDto>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.SearchByUserNameAsync(searchTerm, cancellationToken);
        return users.Select(MapToListDto);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        // Update properties
        if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName != user.UserName)
        {
            // Check if username is already taken
            var existingUser = await _userRepository.GetByUserNameAsync(dto.UserName, cancellationToken);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new InvalidOperationException($"Username '{dto.UserName}' is already taken.");
            }
            user.UserName = dto.UserName;
        }

        if (dto.Bio != null)
        {
            user.Bio = dto.Bio;
        }

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(user);
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        _userRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalUsersCountAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetTotalCountAsync(cancellationToken);
    }

    // Mapping methods
    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.Email.Value,
            user.Bio,
            user.CreatedAt,
            user.Posts.Count,
            user.Comments.Count,
            user.Followers.Count,
            user.Following.Count
        );
    }

    private static UserListDto MapToListDto(User user)
    {
        return new UserListDto(
            user.Id,
            user.UserName,
            user.Email.Value,
            user.Bio
        );
    }
}
