namespace Hotel.Common.Messaging;

public sealed class ConsoleEmailSender : IEmailSender
{
    public void SendBookingConfirmation(string email, string guestName, string reservationId,
        DateTime checkIn, DateTime checkOut, string roomId)
    {
        Console.WriteLine($"  [EMAIL] To: {email}");
        Console.WriteLine($"    Booking confirmed for {guestName}");
        Console.WriteLine($"    Reservation: {reservationId} | Room: {roomId}");
        Console.WriteLine($"    {checkIn:d} → {checkOut:d}");
    }
}
