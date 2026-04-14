namespace Hotel.Booking.Contracts;

public interface IReservationRepository
{
    void Add(Reservation reservation);
    Reservation? GetById(string id);
    List<Reservation> GetByDateRange(DateTime from, DateTime to);
    List<Reservation> GetAll();
    void Update(Reservation reservation);
}
