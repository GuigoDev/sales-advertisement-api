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

    public async Task<IEnumerable<User>> GetUsersAsync()
        => await _databaseContext.Users.AsNoTracking().ToListAsync();
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _databaseContext.Users
            .AsNoTracking()
            .Include(user => user.Advertisements)
            .SingleOrDefaultAsync(user => user.UserId == id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        
        return user;
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        var userToUpdate = await _databaseContext.Users.FindAsync(id);

        if(userToUpdate is null)
            throw new NullReferenceException("User does not exists!");
        else if(user.Email is not null && user.Password is not null)
        {
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            await _databaseContext.SaveChangesAsync();
        }
        else if(user.Email is not null)
        {
            userToUpdate.Email = user.Email;
            await _databaseContext.SaveChangesAsync();
        }
        else if(user.Password is not null)
        {
            userToUpdate.Password = user.Password;
            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var userToDelete = await _databaseContext.Users.FindAsync(id);

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
            await _databaseContext.SaveChangesAsync();
        }

        _databaseContext.Users.Remove(userToDelete);
        await _databaseContext.SaveChangesAsync();
    }
}