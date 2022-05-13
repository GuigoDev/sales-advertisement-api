using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesAnnouncements.Models;

public class Announcement
{   
    public int AnnouncementId { get; set; }

    [MaxLength(60)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public float Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }
}