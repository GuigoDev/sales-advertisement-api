using Amazon;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using SalesAdvertisementApi.Data;
using SalesAdvertisementApi.Models;

namespace SalesAdvertisementApi.Services;

public class AdvertisementService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IAmazonS3 _client = new AmazonS3Client
    (
        new AwsS3BucketServices().Credentials,
        RegionEndpoint.USWest2
    );

    private readonly string _bucketName = new AwsS3BucketServices().BucketName;

    public AdvertisementService(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
    {
        _databaseContext = databaseContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<Advertisement>> GetAdvertisementsAsync()
    {
        return await _databaseContext.Advertisements
            .AsNoTracking()
            .Include(advertisement => advertisement.User)
            .ToListAsync();
    }

    public async Task<Advertisement?> GetAdvertisementAsync(int id)
    {
        return await _databaseContext.Advertisements
            .AsNoTracking()
            .Include(advertisement => advertisement.User)
            .SingleOrDefaultAsync(advertisement => advertisement.AdvertisementId == id);
    }

    public async Task<Advertisement> CreateAdvertisementAsync(IFormFile image, Advertisement advertisement, int userId)
    {   
        var owner = await _databaseContext.Users.FindAsync(userId);
        
        if(owner is null)
            throw new NullReferenceException("User does not exists!");

        if (image is null)
            throw new NullReferenceException("No image to upload!");

        var imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
        var userImagesDirectory = Path.Combine(imagesDirectory, $"{userId}");
        Directory.CreateDirectory(userImagesDirectory);
        
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
        var imageName = $"{timestamp}-{image.FileName}";
        
        var imagePath = Path.Combine(userImagesDirectory, imageName);
        await using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        await AwsS3BucketServices.UploadFileAsync
        (
            _client, 
            _bucketName,
            owner.UserId,
            imageName,
            imagePath
        );
        
        await AwsS3BucketServices.AddAclToExistingObjectAsync
        (
            _client, 
            _bucketName,
            owner.UserId,
            imageName
        );
        
        File.Delete($"{imagePath}");
        
        var newAdvertisement = new Advertisement
        {
            ImageUrl = $"https://{_bucketName}.s3.us-west-2.amazonaws.com/advertisement-images/{owner.UserId}/{imageName}",
            ImageName = imageName,
            Title = advertisement.Title,
            Description = advertisement.Description,
            Price = advertisement.Price,
            CreatedAt = DateTime.UtcNow,
            UserId = owner.UserId,
            User = owner
        };
        
        await _databaseContext.Advertisements.AddAsync(newAdvertisement);
        await _databaseContext.SaveChangesAsync();

        return newAdvertisement;
    }

    public async Task UpdateAdvertisementAsync(int id, Advertisement advertisement)
    {
        var advertisementToUpdate = await _databaseContext.Advertisements.FindAsync(id);

        if(advertisementToUpdate is null)
            throw new NullReferenceException("Advertisement does not exists!");

        var title = advertisement.Title;
        var description = advertisement.Description;
        var price = advertisement.Price;
        
        if(title != null && description != null && price != advertisementToUpdate.Price)
        {
            advertisementToUpdate.Title = title;
            advertisementToUpdate.Description = description;
            advertisementToUpdate.Price = price;

            await _databaseContext.SaveChangesAsync();
        }
        else if(title is not null)
        {
            advertisementToUpdate.Title = title;
            await _databaseContext.SaveChangesAsync();
        }
        else if(description is not null)
        {
            advertisementToUpdate.Description = description;
            await _databaseContext.SaveChangesAsync();
        }
        else if(price != advertisementToUpdate.Price)
        {
            advertisementToUpdate.Price = price;
            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAdvertisementAsync(int id)
    {
        var advertisementToDelete = await _databaseContext.Advertisements.FindAsync(id);
        var owner = await _databaseContext.Users.FindAsync(advertisementToDelete.UserId);
        
        if(advertisementToDelete is null)
            throw new NullReferenceException("Advertisement does not exists!");
        
        var keyName = advertisementToDelete.ImageName;
        
        await AwsS3BucketServices.DeleteObjectAsync
        (
            _client,
            _bucketName,
            owner.UserId,
            keyName
        );
        
        _databaseContext.Advertisements.Remove(advertisementToDelete);
        await _databaseContext.SaveChangesAsync();
    }
}