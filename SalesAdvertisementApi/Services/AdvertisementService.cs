using Microsoft.EntityFrameworkCore;
using SalesAdvertisementApi.Data;
using SalesAdvertisementApi.Models;

namespace SalesAdvertisementApi.Services;

public class AdvertisementService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public AdvertisementService(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
    {
        _databaseContext = databaseContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public List<Advertisement> GetAdvertisements()
    {
        return _databaseContext.Advertisements
            .AsNoTracking()
            .Include(advertisement => advertisement.User)
            .ToList();
    }

    public Advertisement? GetAdvertisement(int id)
    {
       _databaseContext.Advertisements.Find(id);

        return _databaseContext.Advertisements
            .AsNoTracking()
            .Include(advertisement => advertisement.User)
            .SingleOrDefault(advertisement => advertisement.AdvertisementId == id);
    }

    public Advertisement CreateAdvertisement(IFormFile image, Advertisement advertisement, int userId)
    {   
        var owner = _databaseContext.Users.Find(userId);
        
        if(owner is null)
            throw new NullReferenceException("User does not exists!");

        if (image is null)
            throw new NullReferenceException("No image to upload!");

        var imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
        var userImagesDirectory = Path.Combine(imagesDirectory, $"{userId}");
        Directory.CreateDirectory(userImagesDirectory);
        
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
        var imagePath = Path.Combine(userImagesDirectory, $"{timestamp}-{image.FileName}");
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        var newAdvertisement = new Advertisement
        {
            Image = imagePath,
            Title = advertisement.Title,
            Description = advertisement.Description,
            Price = advertisement.Price,
            CreatedAt = DateTime.UtcNow,
            UserId = owner.UserId,
            User = owner
        };
        
        _databaseContext.Advertisements.Add(newAdvertisement);
        _databaseContext.SaveChanges();

        return newAdvertisement;
    }

    public void UpdateAdvertisement(int id, Advertisement advertisement)
    {
        var advertisementToUpdate = _databaseContext.Advertisements.Find(id);

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

            _databaseContext.SaveChanges();
        }
        else if(title is not null)
        {
            advertisementToUpdate.Title = title;
            _databaseContext.SaveChanges();
        }
        else if(description is not null)
        {
            advertisementToUpdate.Description = description;
            _databaseContext.SaveChanges();
        }
        else if(price != advertisementToUpdate.Price)
        {
            advertisementToUpdate.Price = price;
            _databaseContext.SaveChanges();
        }
    }

    public void DeleteAdvertisement(int id)
    {
        var advertisementToDelete = _databaseContext.Advertisements.Find(id);

        if(advertisementToDelete is null)
            throw new NullReferenceException("Advertisement does not exists!");

        if(advertisementToDelete.Image is not null)
        {
            File.Delete(advertisementToDelete.Image);

            _databaseContext.Advertisements.Remove(advertisementToDelete);
            _databaseContext.SaveChanges();
        }

        _databaseContext.Advertisements.Remove(advertisementToDelete);
        _databaseContext.SaveChanges();
    }
}