using WebApplication1.Models;

namespace WebApplication1.Data;

public static class StaticData
{
    public static List<Room> Rooms = new()
    {
        new Room { Id = 1, Name = "A-101", BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "B-204", BuildingCode = "B", Floor = 2, Capacity = 20, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "C-305", BuildingCode = "C", Floor = 3, Capacity = 50, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "A-102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false, IsActive = false }
    };

    public static List<Reservation> Reservations = new()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "C# Basics", Date = DateTime.Today, StartTime = new TimeSpan(9,0,0), EndTime = new TimeSpan(11,0,0), Status = "confirmed" }
    };
}