using SalesAdvertisement.Models;
using SalesAdvertisement.Services;
using Microsoft.AspNetCore.Mvc;

namespace SalesAdvertisement.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IEnumerable<User> Get()
        => _userService.GetUsers();

    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _userService.GetUserById(id);

        if(user is not null)
            return user;
        
        return NotFound("User does not exists!");
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Create(User user)
    {
        _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetById), new { id = user!.UserId }, user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, User user)
    {
        var userToUpdate = _userService.GetUserById(id);

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
        var userToDelete = _userService.GetUserById(id);

        if(userToDelete is not null)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }

        return NotFound("User does not exists!");
    }
}