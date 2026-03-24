namespace Hotel;

public class InMemoryReservationStore : IReservationRepository
{
    private readonly Dictionary<string, Reservation> _store = new();

    public void Add(Reservation reservation) => _store[reservation.Id] = reservation;
    public Reservation? GetById(string id) => _store.GetValueOrDefault(id);
    public List<Reservation> GetAll() => _store.Values.ToList();
    public void Update(Reservation reservation) => _store[reservation.Id] = reservation;

    public List<Reservation> GetByDateRange(DateTime from, DateTime to) =>
        _store.Values.Where(r =>
            r.Status != "Cancelled" && r.CheckIn < to && r.CheckOut > from).ToList();
}
