namespace Hotel.Booking.Contracts;

public class Room
{
    public string Id { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
}
