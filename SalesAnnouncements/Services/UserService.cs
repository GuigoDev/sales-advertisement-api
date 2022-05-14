using SalesAnnouncements.Models;
using SalesAnnouncements.Data;
using Microsoft.EntityFrameworkCore;

namespace SalesAnnouncements.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public IEnumerable<User> GetUsers()
        => _databaseContext.Users.AsNoTracking().ToList();
    
    public User? GetUser(int id)
    {
        return _databaseContext.Users
            .AsNoTracking()
            .Include(user => user.Announcements)
            .SingleOrDefault(user => user.UserId == id);
    }

    public User CreateUser(User user)
    {
        _databaseContext.Users.Add(user);
        _databaseContext.SaveChanges();

        return user;
    }

    public void UpdateUser(int id, User user)
    {
        var userToUpdate = _databaseContext.Users.Find(id);

        if(userToUpdate is null)
            throw new NullReferenceException("User does not exists!");
        else if(user.Email is not null && user.Password is not null)
        {
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            _databaseContext.SaveChanges();
        }
        else if(user.Email is not null)
        {
            userToUpdate.Email = user.Email;
            _databaseContext.SaveChanges();
        }
        else if(user.Password is not null)
        {
            userToUpdate.Password = user.Password;
            _databaseContext.SaveChanges();
        }
    }

    public void DeleteUser(int id)
    {
        var userToDelete = _databaseContext.Users.Find(id);

        if(userToDelete is null)
            throw new NullReferenceException("User does not exists!");

        _databaseContext.Users.Remove(userToDelete);
        _databaseContext.SaveChanges();
    }
}