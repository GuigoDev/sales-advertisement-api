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
            .ToList();
    }

    public Announcement? GetAnnouncement(int id)
    {
        return _databaseContext.Announcements
            .AsNoTracking()
            .Include(announcement => announcement.User)
            .SingleOrDefault(announcement => announcement.AnnouncementId == id);
    }

    public Announcement CreateAnnoucement(Announcement announcement, int id)
    {   
        var owner = _databaseContext.Users.Find(id);

        if(owner is null)
            throw new NullReferenceException("User does not exists!");

        var user = _databaseContext.Announcements.Include(user => user.User).ToList();

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

    public void DeleteAnnouncement(int id)
    {
        var announcementToDelete = _databaseContext.Announcements.Find(id);

        if(announcementToDelete is null)
            throw new NullReferenceException("Announcement does not exists!");

        _databaseContext.Announcements.Remove(announcementToDelete);
        _databaseContext.SaveChanges();
    }
}