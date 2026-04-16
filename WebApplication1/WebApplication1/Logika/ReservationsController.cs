using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(DateTime? date, string? status, int? roomId)
    {
        var query = StaticData.Reservations.AsQueryable();
        if (date.HasValue) query = query.Where(r => r.Date.Date == date.Value.Date);
        if (!string.IsNullOrEmpty(status)) query = query.Where(r => r.Status == status);
        if (roomId.HasValue) query = query.Where(r => r.RoomId == roomId);

        return Ok(query.ToList());
    }

    [HttpPost]
    public IActionResult Create(Reservation res)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == res.RoomId);
        
        // 1. Czy sala istnieje i jest aktywna?
        if (room == null) return NotFound("Sala nie istnieje.");
        if (!room.IsActive) return BadRequest("Sala jest nieaktywna.");

        // 2. Czy nie ma konfliktu czasowego?
        var overlap = StaticData.Reservations.Any(r => 
            r.RoomId == res.RoomId && 
            r.Date.Date == res.Date.Date &&
            r.StartTime < res.EndTime && res.StartTime < r.EndTime);

        if (overlap) return Conflict("Sala jest już zarezerwowana w tym czasie.");

        res.Id = StaticData.Reservations.Count > 0 ? StaticData.Reservations.Max(r => r.Id) + 1 : 1;
        StaticData.Reservations.Add(res);
        return CreatedAtAction(nameof(Get), new { roomId = res.RoomId }, res);
    }
}