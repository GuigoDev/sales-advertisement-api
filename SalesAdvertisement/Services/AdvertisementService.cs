using SalesAdvertisement.Models;
using SalesAdvertisement.Data;
using Microsoft.EntityFrameworkCore;

namespace SalesAdvertisement.Services;

public class AdvertisementService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public AdvertisementService(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
    {
        _databaseContext = databaseContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public List<Advertisement> GetAdvertisement()
    {
        return _databaseContext.Advertisements
            .AsNoTracking()
            .Include(advertisement => advertisement.User)
            .ToList();
    }

    public Advertisement? GetAdvertisement(int id)
    {
       var announcement = _databaseContext.Advertisements.Find(id);

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

        var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
        var fullDirectoryPath = Path.Combine(directoryPath, $"{userId}");
        Directory.CreateDirectory(fullDirectoryPath);

        var filePath = Path.Combine(fullDirectoryPath, image.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        owner.Password = "";

        var newAdvertisement = new Advertisement
        {
            Images = filePath,
            Title = advertisement.Title,
            Description = advertisement.Description,
            Price = advertisement.Price,
            CreatedAt = DateTime.Now.Date,
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

        _databaseContext.Advertisements.Remove(advertisementToDelete);
        _databaseContext.SaveChanges();
    }
}