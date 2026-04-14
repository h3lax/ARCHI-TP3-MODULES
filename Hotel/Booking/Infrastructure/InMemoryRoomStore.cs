using Hotel.Booking.Contracts;

namespace Hotel.Booking.Infrastructure;

public class InMemoryRoomStore : IRoomRepository
{
    private readonly List<Room> _rooms;

    public InMemoryRoomStore(List<Room> rooms) => _rooms = rooms;

    public Room? GetById(string id) => _rooms.FirstOrDefault(r => r.Id == id);
    public List<Room> GetAll() => _rooms.ToList();

    public List<Room> GetAvailable(DateTime from, DateTime to, List<Reservation> existing)
    {
        var bookedRoomIds = existing
            .Where(r => r.Status != "Cancelled" && r.CheckIn < to && r.CheckOut > from)
            .Select(r => r.RoomId)
            .ToHashSet();

        return _rooms.Where(r => !bookedRoomIds.Contains(r.Id)).ToList();
    }
}
