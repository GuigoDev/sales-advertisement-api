using SalesAnnouncements.Models;
using SalesAnnouncements.Data;
using Microsoft.EntityFrameworkCore;

namespace SalesAnnouncements.Services;

public class AnnouncementService
{
    private readonly DatabaseContext _databaseContext;

    public AnnouncementService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
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

    public Announcement CreateAnnoucement(Announcement announcement, int userId)
    {   
        var owner = _databaseContext.Users.Find(userId);
        
        if(owner is null)
            throw new NullReferenceException("User does not exists!");

        owner.Password = "";

        var newAnnoucement = new Announcement 
        {
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
             announcementToUpdate.Title = title ?? announcementToUpdate.Title;
             announcementToUpdate.Description = description ?? announcementToUpdate.Description;
             announcementToUpdate.Price = price;

            _databaseContext.SaveChanges();
        }
        else if(title is not null)
        {
            announcementToUpdate.Title = title ?? announcementToUpdate.Title;
            _databaseContext.SaveChanges();
        }
        else if(description is not null)
        {
            announcementToUpdate.Description = description ?? announcementToUpdate.Description;
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