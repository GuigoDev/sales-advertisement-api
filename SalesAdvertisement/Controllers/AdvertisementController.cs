using SalesAdvertisement.Models;
using SalesAdvertisement.Services;
using Microsoft.AspNetCore.Mvc;

namespace SalesAnnouncements.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AnnouncementController : ControllerBase
{
    AdvertisementService _advertisementService;

    public AnnouncementController(AdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpGet]
    public List<Advertisement> Get()
        => _advertisementService.GetAdvertisement();

    [HttpGet("{id}")]
    public ActionResult<Advertisement> GetById(int id)
    {
        var advertisement = _advertisementService.GetAdvertisement(id);

        if(advertisement is not null)
            return advertisement;

        return NotFound();
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromForm] IFormFile image, [FromForm] Advertisement advertisement, [FromHeader] int userId)
    {
        var newAdvertisement = _advertisementService.CreateAdvertisement(image, advertisement, userId);

        return CreatedAtAction(
            nameof(GetById), new { id = newAdvertisement!.AdvertisementId }, newAdvertisement
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Advertisement advertisement)
    {
        var advertisementToUpdate = _advertisementService.GetAdvertisement(id);

        if(advertisementToUpdate is not null)
        {
            _advertisementService.UpdateAdvertisement(id, advertisement);
            return NoContent();
        }
        else
            return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var advertisementToDelete = _advertisementService.GetAdvertisement(id);

        if(advertisementToDelete is not null)
        {
            _advertisementService.DeleteAdvertisement(id);
            return NoContent();
        }
        
        return NotFound();
    }
}