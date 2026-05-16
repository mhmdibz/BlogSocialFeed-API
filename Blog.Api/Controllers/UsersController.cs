using Blog.Application.DTOs;
using Blog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

     [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetAllUsersAsync(pageNumber, pageSize, cancellationToken);
        return Ok(users);
    }

     [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        return user == null ? NotFound(new { Message = $"User {id} not found." }) : Ok(user);
    }

     [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<UserListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string term, CancellationToken cancellationToken = default)
    {
        var users = await _userService.SearchUsersAsync(term, cancellationToken);
        return Ok(users);
    }

     [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userService.CreateUserAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

     [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userService.UpdateUserAsync(id, dto, cancellationToken);
        return Ok(user);
    }

     [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        await _userService.DeleteUserAsync(id, cancellationToken);
        return NoContent();
    }

     [HttpGet("count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken = default)
    {
        var count = await _userService.GetTotalUsersCountAsync(cancellationToken);
        return Ok(new { Count = count });
    }
}
