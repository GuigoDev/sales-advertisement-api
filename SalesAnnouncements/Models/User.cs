using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesAnnouncements.Models;

public class User
{   
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    [MaxLength(30)]
    [JsonIgnore]
    public string? Password { get; set; }
    
    public List<Announcement>? Announcements { get; set; }
}