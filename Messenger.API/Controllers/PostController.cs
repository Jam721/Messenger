using System.IdentityModel.Tokens.Jwt;
using Messenger.Application.Interfaces;
using Messenger.Contracts;
using Messenger.Core.Models;
using Messenger.Core.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IPostService _postService;
    private readonly IUserRepository _userRepository;

    public PostController(IPostRepository postRepository, IPostService postService, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _postService = postService;
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postRepository.GetAllAsync();

        if (posts == null) return BadRequest("Нет постов");

        var postsDto = posts.Select(p => new
        {
            Id = p.Id,
            Title = p.Title,
            CreatedAt = p.CreatedAt,
            Description = p.Description,
            UserName = p.User.UserName
        });
        
        return Ok(postsDto);
    }

    [Authorize]
    [HttpGet]
    [Route("GetById/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post == null) return BadRequest("Пост не найден");

        return Ok(new PostDto
        {
            Title = post.Title,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            UserName = post.User.UserName,
            Id = post.Id
        });
    }

    [Authorize]
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] CreatePostDto postDto)
    {
        var count = await _postService.GetNextPostId();
        var token = Request.Cookies["tasty-cookies"];
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized();
    
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId"); 
        if (userIdClaim == null) return Unauthorized();
    
        var userId = Guid.Parse(userIdClaim.Value);
        var user = await _userRepository.GetByIdAsync(userId);
    
        if (user == null) return NotFound();

        var post = new Post
        {
            Id = count,
            Title = postDto.Title,
            Description = postDto.Description,
            CreatedAt = DateTime.UtcNow,
            User = user
        };
        
        user.Posts.Add(post);
    
        var createdPost = await _postRepository.CreateAsync(post);
        
        return Ok(new PostDto
        {
            Title = createdPost.Title,
            CreatedAt = createdPost.CreatedAt,
            Description = createdPost.Description,
            UserName = createdPost.User.UserName,
            Id = createdPost.Id
        });
    }


    [Authorize]
    [HttpPut]
    [Route("Update/{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreatePostDto postDto)
    {
        var newPost = new Post
        {
            Description = postDto.Description,
            Title = postDto.Title
        };
        var post = await _postRepository.UpdateAsync(id, newPost);
        
        if(post==null) return BadRequest("Пост не найден");

        return Ok(new PostDto
        {
            Title = post.Title,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            UserName = post.User.UserName,
            Id = post.Id
        });
    }
    
    [Authorize]
    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var post = await _postRepository.DeleteAsync(id);
        
        if(post==null) return BadRequest("Пост не найден");

        return Ok(new PostDto
        {
            Title = post.Title,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            UserName = post.User.UserName,
            Id = post.Id
        });
    }
    
    [HttpGet("GetByUser/{username}")]
    public async Task<IActionResult> GetByUser(string username)
    {
        var posts = await _postRepository.GetAllAsync(); // Получаем все посты

        var userPosts = posts.Where(p => p.User.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!userPosts.Any())
        {
            return NotFound(); // Возвращаем 404, если посты не найдены
        }

        var postsDto = userPosts.Select(p => new
        {
            Id = p.Id,
            Title = p.Title,
            CreatedAt = p.CreatedAt,
            Description = p.Description,
            UserName = p.User.UserName
        });

        return Ok(postsDto);
    }

}