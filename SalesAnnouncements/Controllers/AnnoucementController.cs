using SalesAnnouncements.Models;
using SalesAnnouncements.Services;
using Microsoft.AspNetCore.Mvc;

namespace SalesAnnouncements.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AnnouncementController : ControllerBase
{
    AnnouncementService _announcementService;

    public AnnouncementController(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public List<Announcement> Get()
        => _announcementService.GetAnnouncements();

    [HttpGet("{id}")]
    public ActionResult<Announcement> GetById(int id)
    {
        var announcement = _announcementService.GetAnnouncement(id);

        if(announcement is not null)
            return announcement;

        return NotFound();
    }

    [HttpPost]
    public IActionResult Create([FromBody]Announcement announcement, [FromHeader] int userId)
    {
        var newAnnouncement = _announcementService.CreateAnnoucement(announcement, userId);

        return CreatedAtAction(
            nameof(GetById), new { id = newAnnouncement!.AnnouncementId}, newAnnouncement
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Announcement announcement)
    {
        var announcementToUpdate = _announcementService.GetAnnouncement(id);

        if(announcementToUpdate is not null)
        {
            _announcementService.UpdateAnnouncement(id, announcement);
            return NoContent();
        }
        else
            return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var announcementToDelete = _announcementService.GetAnnouncement(id);

        if(announcementToDelete is not null)
        {
            _announcementService.DeleteAnnouncement(id);
            return NoContent();
        }
        
        return NotFound();
    }
}