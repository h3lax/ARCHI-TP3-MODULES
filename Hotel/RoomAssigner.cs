namespace Hotel;

public class RoomAssigner
{
    private readonly IRoomRepository _roomRepo;
    private readonly IReservationRepository _reservationRepo;

    public RoomAssigner(IRoomRepository roomRepo, IReservationRepository reservationRepo)
    {
        _roomRepo = roomRepo;
        _reservationRepo = reservationRepo;
    }

    public Room? FindAvailableRoom(RoomType type, DateTime checkIn, DateTime checkOut, int guestCount)
    {
        var existing = _reservationRepo.GetAll();
        var available = _roomRepo.GetAvailable(checkIn, checkOut, existing);
        return available.FirstOrDefault(r => r.Type == type && r.Capacity >= guestCount);
    }
}
