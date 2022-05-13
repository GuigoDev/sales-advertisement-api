using SalesAnnouncements.Models;
using SalesAnnouncements.Services;
using Microsoft.AspNetCore.Mvc;

namespace SalesAnnouncements.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IEnumerable<User> GetUsers()
        => _userService.GetUsers();

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = _userService.GetUser(id);

        if(user is not null)
            return user;
        
        return NotFound("User does not exists!");
    }

    [HttpPost]
    [Route("SignIn")]
    public IActionResult Create(User user)
    {
        var userToCreate = _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetUser), new { id = user!.UserId }, user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, User user)
    {
        var userToUpdate = _userService.GetUser(id);

        if(userToUpdate is not null)
        {
            _userService.UpdateUser(id, user);
            return NoContent();
        }
        
        return NotFound("User does not exists!");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var userToDelete = _userService.GetUser(id);

        if(userToDelete is not null)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }

        return NotFound("User does not exists!");
    }
}