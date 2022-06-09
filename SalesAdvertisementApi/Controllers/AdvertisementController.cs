using Microsoft.AspNetCore.Mvc;
using SalesAdvertisementApi.Models;
using SalesAdvertisementApi.Services;

namespace SalesAdvertisementApi.Controllers;

[ApiController]
[Route("/[controller]")]
public class AdvertisementController : ControllerBase
{
    AdvertisementService _advertisementService;

    public AdvertisementController(AdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpGet]
    public async Task<List<Advertisement>> Get()
        => await _advertisementService.GetAdvertisementsAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Advertisement>> GetById(int id)
    {
        var advertisement = await _advertisementService.GetAdvertisementAsync(id);

        if(advertisement is not null)
            return advertisement;

        return NotFound();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromForm] IFormFile image, [FromForm] Advertisement advertisement, [FromHeader] int userId)
    {
        var newAdvertisement = await _advertisementService.CreateAdvertisementAsync(image, advertisement, userId);

        return CreatedAtAction(
            nameof(GetById), new { id = newAdvertisement.AdvertisementId }, newAdvertisement
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Advertisement advertisement)
    {
        var advertisementToUpdate = await _advertisementService.GetAdvertisementAsync(id);

        if(advertisementToUpdate is null)
            return NotFound();
        
        await _advertisementService.UpdateAdvertisementAsync(id, advertisement);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var advertisementToDelete = await _advertisementService.GetAdvertisementAsync(id);

        if(advertisementToDelete is null)
            return NotFound();
        
        await _advertisementService.DeleteAdvertisementAsync(id);
        return NoContent();
    }
}