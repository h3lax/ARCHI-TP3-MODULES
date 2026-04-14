namespace Hotel.Booking.Contracts;

public interface IRoomRepository
{
    Room? GetById(string id);
    List<Room> GetAll();
    List<Room> GetAvailable(DateTime from, DateTime to, List<Reservation> existing);
}
