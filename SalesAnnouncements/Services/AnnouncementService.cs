using SalesAnnouncements.Models;
using SalesAnnouncements.Data;
using Microsoft.EntityFrameworkCore;

namespace SalesAnnouncements.Services;

public class AnnouncementService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public AnnouncementService(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
    {
        _databaseContext = databaseContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public List<Announcement> GetAnnouncements()
    {
        return _databaseContext.Announcements
            .AsNoTracking()
            .Include(announcement => announcement.User)
            .ToList();
    }

    public Announcement? GetAnnouncement(int id)
    {
       var announcement = _databaseContext.Announcements.Find(id);

        return _databaseContext.Announcements
            .AsNoTracking()
            .Include(announcement => announcement.User)
            .SingleOrDefault(announcement => announcement.AnnouncementId == id);
    }

    public Announcement CreateAnnoucement(IFormFile image,Announcement announcement, int userId)
    {   
        var owner = _databaseContext.Users.Find(userId);
        
        if(owner is null)
            throw new NullReferenceException("User does not exists!");

        owner.Password = "";

        string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
        string filePath = "";

        
        filePath = Path.Combine(directoryPath, image.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }
        

        var newAnnoucement = new Announcement
        {
            Images = $"{filePath}",
            Title = announcement.Title,
            Description = announcement.Description,
            Price = announcement.Price,
            CreatedAt = DateTime.Now.Date,
            UserId = owner.UserId,
            User = owner
        };
        
        _databaseContext.Announcements.Add(newAnnoucement);
        _databaseContext.SaveChanges();

        return newAnnoucement;
    }

    public void UpdateAnnouncement(int id, Announcement announcement)
    {
        var announcementToUpdate = _databaseContext.Announcements.Find(id);

        if(announcementToUpdate is null)
            throw new NullReferenceException("Announcement does not exists!");

        var title = announcement.Title;
        var description = announcement.Description;
        var price = announcement.Price;
        
        if(title != null && description != null && price != announcementToUpdate.Price)
        {
             announcementToUpdate.Title = title;
             announcementToUpdate.Description = description;
             announcementToUpdate.Price = price;

            _databaseContext.SaveChanges();
        }
        else if(title is not null)
        {
            announcementToUpdate.Title = title;
            _databaseContext.SaveChanges();
        }
        else if(description is not null)
        {
            announcementToUpdate.Description = description;
            _databaseContext.SaveChanges();
        }
        else if(price != announcementToUpdate.Price)
        {
             announcementToUpdate.Price = price;
            _databaseContext.SaveChanges();
        }
    }

    public void DeleteAnnouncement(int id)
    {
        var announcementToDelete = _databaseContext.Announcements.Find(id);

        if(announcementToDelete is null)
            throw new NullReferenceException("Announcement does not exists!");

        _databaseContext.Announcements.Remove(announcementToDelete);
        _databaseContext.SaveChanges();
    }
}