using System.ComponentModel.DataAnnotations;

namespace SalesAdvertisement.Models;

public class Advertisement
{   
    public int AdvertisementId { get; set; }

    public string? Image { get; set; }

    [MaxLength(60)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }
}