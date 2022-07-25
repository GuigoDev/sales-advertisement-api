using Microsoft.AspNetCore.Mvc;
using SalesAdvertisementApi.Models;
using SalesAdvertisementApi.Services;

namespace SalesAdvertisementApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("skip/{skip:int}/take/{take:int}")]
    public async Task<IEnumerable<User>> Get([FromRoute] int skip, [FromRoute] int take)
        => await _userService.GetUsersAsync(skip, take);

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if(user is not null)
            return user;
        
        return NotFound("User does not exists!");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Create(User user)
    {
        var userEmail = await _userService.GetUserByEmailAsync($"{user.Email}");

        if (userEmail)
            return BadRequest("This email has already been registered.");
        
        await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        var userToUpdate = await _userService.GetUserByIdAsync(id);

        if(userToUpdate is null)
            return NotFound("User does not exists!");
        
        await _userService.UpdateUserAsync(id, user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userToDelete = await _userService.GetUserByIdAsync(id);

        if(userToDelete is null)
            return NotFound("User does not exists!");
        
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}