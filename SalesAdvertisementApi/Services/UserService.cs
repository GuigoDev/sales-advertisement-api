using Amazon;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using SalesAdvertisementApi.Data;
using SalesAdvertisementApi.Models;

namespace SalesAdvertisementApi.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;
    
    private readonly IAmazonS3 _client = new AmazonS3Client
    (
        new AwsS3Services().Credentials, 
        RegionEndpoint.USWest2
    );

    private readonly string _bucketName = new AwsS3Services().BucketName;
    
    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(int skip, int take)
        => await _databaseContext.Users
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _databaseContext.Users
            .AsNoTracking()
            .Include(user => user.Advertisements)
            .SingleOrDefaultAsync(user => user.UserId == id);
    }

    public async Task<bool> GetUserByEmailAsync(string email)
    {
        var userEmail = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Email == email);

        if (userEmail is null)
            return false;

        return true;
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

        if(!String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Password))
        {
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            await _databaseContext.SaveChangesAsync();
        }
        else if(!String.IsNullOrEmpty(user.Email))
        {
            userToUpdate.Email = user.Email;
            await _databaseContext.SaveChangesAsync();
        }
        else if(!String.IsNullOrEmpty(user.Password))
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
        
        var userImages = await AwsS3Services.ListObjectsAsync(_client, _bucketName, userToDelete.UserId);
        var count = 0;

        while (count < userImages.S3Objects.Count)
        {
            count++;
        }
        
        if (count > 0)
        {
            await AwsS3Services.DeleteUserFoldAsync(_client, _bucketName, userToDelete.UserId);
            
            _databaseContext.Users.Remove(userToDelete);
            await _databaseContext.SaveChangesAsync();
        }

        _databaseContext.Users.Remove(userToDelete);
        await _databaseContext.SaveChangesAsync();
    }
}