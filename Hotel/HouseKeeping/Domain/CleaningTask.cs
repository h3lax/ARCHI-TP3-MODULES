namespace Hotel.HouseKeeping.Domain;

public class CleaningTask
{
    public string RoomId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string ReservationId { get; set; } = string.Empty;
}
