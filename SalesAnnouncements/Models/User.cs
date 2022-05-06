using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesAnnouncements.Models;

public class User
{
    public int UserID { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(25)]
    public string Password { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Announcement>? Announcement { get; set; }
}