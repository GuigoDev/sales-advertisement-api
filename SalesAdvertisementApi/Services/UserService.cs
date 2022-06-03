using Microsoft.EntityFrameworkCore;
using SalesAdvertisementApi.Data;
using SalesAdvertisementApi.Models;

namespace SalesAdvertisementApi.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public IEnumerable<User> GetUsers()
        => _databaseContext.Users.AsNoTracking().ToList();
    
    public User? GetUserById(int id)
    {
        return _databaseContext.Users
            .AsNoTracking()
            .Include(user => user.Advertisements)
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

        var imagesDirectory = Path.Combine(
            Directory.GetCurrentDirectory(), $"Images/{Path.DirectorySeparatorChar}{userToDelete.UserId}"
        );

        if(Directory.Exists(imagesDirectory))
        {
            IEnumerable<string> images = Directory.EnumerateFiles(imagesDirectory, "*");

            foreach(string image in images)
            {
                File.Delete(image);
            }

            Directory.Delete(imagesDirectory);

            _databaseContext.Users.Remove(userToDelete);
            _databaseContext.SaveChanges();
        }

        _databaseContext.Users.Remove(userToDelete);
        _databaseContext.SaveChanges();
    }
}