using Blog.Application.DTOs;
using Blog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReactionsController : ControllerBase
{
    private readonly ReactionService _reactionService;

    public ReactionsController(ReactionService reactionService)
    {
        _reactionService = reactionService;
    }

     [HttpGet("post/{postId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ReactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPost(int postId, CancellationToken cancellationToken = default)
    {
        var reactions = await _reactionService.GetReactionsByPostIdAsync(postId, cancellationToken);
        return Ok(reactions);
    }

     [HttpGet("post/{postId:int}/counts")]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountsByPost(int postId, CancellationToken cancellationToken = default)
    {
        var counts = await _reactionService.GetReactionCountsByPostIdAsync(postId, cancellationToken);
        return Ok(counts);
    }

     [HttpPost]
    [ProducesResponseType(typeof(ReactionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> React([FromBody] CreateReactionDto dto, CancellationToken cancellationToken = default)
    {
        var reaction = await _reactionService.ReactAsync(dto, cancellationToken);
        return Created(string.Empty, reaction);
    }

     [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken = default)
    {
        await _reactionService.RemoveReactionAsync(id, cancellationToken);
        return NoContent();
    }
}
