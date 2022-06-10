using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesAdvertisementApi.Models;

public class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    [MaxLength(30)]
    [JsonIgnore]
    public string? Password { get; set; }
    
    public string? BucketName { get; set; }
    
    public List<Advertisement>? Advertisements { get; set; }
}