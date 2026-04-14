using Hotel.Booking.Contracts;
using Hotel.Common.Messaging;

namespace Hotel.Booking.Application;

public class BookingService
{
    private readonly IReservationRepository _reservationRepo;
    private readonly RoomAssigner _roomAssigner;
    private readonly IEmailSender _emailSender;
    private int _counter = 0;

    public BookingService(
        IReservationRepository reservationRepo,
        RoomAssigner roomAssigner,
        IEmailSender emailSender)
    {
        _reservationRepo = reservationRepo;
        _roomAssigner = roomAssigner;
        _emailSender = emailSender;
    }

    public Reservation CreateReservation(string guestName, RoomType roomType,
        DateTime checkIn, DateTime checkOut, int guestCount,
        string email, string phone, string cancellationPolicy = "Flexible")
    {
        if (checkOut <= checkIn)
            throw new ArgumentException("Check-out must be after check-in");

        var room = _roomAssigner.FindAvailableRoom(roomType, checkIn, checkOut, guestCount)
            ?? throw new Exception($"No {roomType} room available for {checkIn:d} → {checkOut:d}");

        if (guestCount > room.Capacity)
            throw new Exception($"Room {room.Id} capacity is {room.Capacity}, requested {guestCount}");

        _counter++;
        var reservation = new Reservation
        {
            Id = $"R-{_counter:D3}",
            GuestName = guestName,
            RoomId = room.Id,
            RoomType = roomType,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            GuestEmail = email,
            GuestPhone = phone,
            CancellationPolicyName = cancellationPolicy
        };

        _reservationRepo.Add(reservation);

        _emailSender.SendBookingConfirmation(
            email, guestName, reservation.Id, checkIn, checkOut, room.Id);

        Console.WriteLine($"  Reservation {reservation.Id} created for {guestName} in room {room.Id}");

        return reservation;
    }

    public void CheckIn(string reservationId)
    {
        var reservation = _reservationRepo.GetById(reservationId)
            ?? throw new Exception($"Reservation {reservationId} not found");

        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: status is {reservation.Status}");

        reservation.Status = "CheckedIn";
        _reservationRepo.Update(reservation);
        Console.WriteLine($"  Guest {reservation.GuestName} checked in to room {reservation.RoomId}");
    }

    public void CheckOut(string reservationId)
    {
        var reservation = _reservationRepo.GetById(reservationId)
            ?? throw new Exception($"Reservation {reservationId} not found");

        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: status is {reservation.Status}");

        reservation.Status = "CheckedOut";
        _reservationRepo.Update(reservation);
        Console.WriteLine($"  Guest {reservation.GuestName} checked out of room {reservation.RoomId}");
    }
}
