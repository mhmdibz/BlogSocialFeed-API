using Blog.Application.DTOs;
using Blog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }

     [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentService.GetCommentByIdAsync(id, cancellationToken);
        return comment == null ? NotFound(new { Message = $"Comment {id} not found." }) : Ok(comment);
    }

     [HttpGet("post/{postId:int}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPost(int postId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentService.GetCommentsByPostIdAsync(postId, cancellationToken);
        return Ok(comments);
    }

     [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = await _commentService.CreateCommentAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
    }

     [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = await _commentService.UpdateCommentAsync(id, dto, cancellationToken);
        return Ok(comment);
    }

     [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        await _commentService.DeleteCommentAsync(id, cancellationToken);
        return NoContent();
    }
}
