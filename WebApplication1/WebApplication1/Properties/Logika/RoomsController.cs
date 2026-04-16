using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var query = StaticData.Rooms.AsQueryable();

        if (minCapacity.HasValue) query = query.Where(r => r.Capacity >= minCapacity);
        if (hasProjector.HasValue) query = query.Where(r => r.HasProjector == hasProjector);
        if (activeOnly == true) query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        return room == null ? NotFound() : Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = StaticData.Rooms.Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase));
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        room.Id = StaticData.Rooms.Max(r => r.Id) + 1;
        StaticData.Rooms.Add(room);
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Room updatedRoom)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        if (StaticData.Reservations.Any(res => res.RoomId == id))
            return Conflict("Nie można usunąć sali z rezerwacjami.");

        StaticData.Rooms.Remove(room);
        return NoContent();
    }
}