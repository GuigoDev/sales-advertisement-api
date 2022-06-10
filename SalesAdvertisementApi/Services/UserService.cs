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
        new AwsS3BucketServices().Credentials, 
        RegionEndpoint.USWest2
    );
    
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
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
        var bucketName = $"{timestamp}-bucket";
        
        await AwsS3BucketServices.CreateBucketAsync(_client, bucketName);

        user.BucketName = bucketName;
        
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

        await AwsS3BucketServices.DeleteBucketContentsAsync
        (
            _client,
            $"{userToDelete.BucketName}"
        );
        
        await AwsS3BucketServices.DeleteBucketAsync
        (
            _client, 
            $"{userToDelete.BucketName}"
        );

        _databaseContext.Users.Remove(userToDelete);
        await _databaseContext.SaveChangesAsync();
    }
}