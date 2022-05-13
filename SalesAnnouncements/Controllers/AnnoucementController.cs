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

    

    [HttpGet("{id}")]
    public ActionResult<Announcement> GetById(int id)
    {
        var announcement = _announcementService.GetAnnouncement(id);

        if(announcement is not null)
            return announcement;

        return NotFound();
    }

    [HttpPost]
    public IActionResult Create([FromBody]Announcement announcement, [FromHeader] int id)
    {
        var newAnnouncement = _announcementService.CreateAnnoucement(announcement, id);

        return CreatedAtAction(
            nameof(GetById), new { id = newAnnouncement!.AnnouncementId}, newAnnouncement
        );
    }
}