namespace Hotel;

public class Reservation
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; } = "Confirmed";
    public string CancellationPolicyName { get; set; } = "Flexible";
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;

    public int Nights => (CheckOut - CheckIn).Days;
}
