using System.ComponentModel.DataAnnotations;

namespace SalesAdvertisementApi.Models;

public class Advertisement
{   
    public int AdvertisementId { get; set; }

    public string? ImageUrl { get; set; }
    
    public string? ImageName { get; set;  }

    [MaxLength(60)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }
}