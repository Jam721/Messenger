using Messenger.Dtos;
using Messenger.Helpers.Post;
using Messenger.Interfaces;
using Messenger.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepositortes _postRepo;

    public PostController(IPostRepositortes postRepo)
    {
        _postRepo = postRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PostQuery query)
    {
        var posts = await _postRepo.GetAllAsync(query);

        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostDto postDto)
    {
        var post = postDto.PostDtoToPost();
        
        post.Id = await _postRepo.GetLastIdAsync();
        post.CreatedAt = DateTime.UtcNow;

        await _postRepo.CreateAsync(post);
        return Ok(post.PostToPostDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PostDto postDto)
    {
        var post = await _postRepo.UpdateAsync(id, postDto);

        if (post == null) return NotFound("Пост не найден");

        return Ok(post.PostToPostDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var post = await _postRepo.DeleteAsync(id);

        if (post == null) return NotFound("Пост не найден");
        
        return Ok(post.PostToPostDto());
    }
    

}