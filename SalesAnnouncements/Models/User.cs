using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesAnnouncements.Models;

public class User
{
    public int UserID { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    [MaxLength(25)]
    public string? Password { get; set; }

    [JsonIgnore]
    public ICollection<Announcement>? Announcement { get; set; }
}