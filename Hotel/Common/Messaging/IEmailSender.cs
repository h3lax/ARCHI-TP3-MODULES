namespace Hotel.Common.Messaging;

public interface IEmailSender
{
    void SendBookingConfirmation(string email, string guestName, string reservationId,
        DateTime checkIn, DateTime checkOut, string roomId);
}
