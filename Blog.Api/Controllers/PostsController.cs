using Blog.Application.DTOs;
using Blog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly PostService _postService;

    public PostsController(PostService postService)
    {
        _postService = postService;
    }

     [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var posts = await _postService.GetAllPostsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(posts);
    }

     [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var post = await _postService.GetPostByIdAsync(id, cancellationToken);
        return post == null ? NotFound(new { Message = $"Post {id} not found." }) : Ok(post);
    }

     [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(
        int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var posts = await _postService.GetPostsByUserIdAsync(userId, pageNumber, pageSize, cancellationToken);
        return Ok(posts);
    }

     [HttpPost]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken cancellationToken = default)
    {
        var post = await _postService.CreatePostAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

     [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDto dto, CancellationToken cancellationToken = default)
    {
        var post = await _postService.UpdatePostAsync(id, dto, cancellationToken);
        return Ok(post);
    }

     [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        await _postService.DeletePostAsync(id, cancellationToken);
        return NoContent();
    }
}
