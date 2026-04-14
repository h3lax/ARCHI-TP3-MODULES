using Hotel.Billing.Pricing;
using Hotel.Billing.Tax;
using Hotel.Booking.Contracts;

namespace Hotel.Billing.Invoicing;

public class InvoiceGenerator
{
    private readonly PricingStrategyFactory _pricingFactory;
    private readonly TaxCalculator _taxCalculator;
    private readonly IRoomRepository _roomRepo;

    public InvoiceGenerator(
        PricingStrategyFactory pricingFactory,
        TaxCalculator taxCalculator,
        IRoomRepository roomRepo)
    {
        _pricingFactory = pricingFactory;
        _taxCalculator = taxCalculator;
        _roomRepo = roomRepo;
    }

    public Invoice Generate(Reservation reservation)
    {
        var room = _roomRepo.GetById(reservation.RoomId)
            ?? throw new Exception($"Room {reservation.RoomId} not found");

        var pricing = _pricingFactory.Create(reservation.RoomType);
        var nightRate = pricing.CalculateNightRate(room);
        var subtotal = nightRate * reservation.Nights;
        var tva = _taxCalculator.CalculateTva(subtotal);
        var touristTax = _taxCalculator.CalculateTouristTax(
            reservation.GuestCount, reservation.Nights);

        return new Invoice
        {
            ReservationId = reservation.Id,
            GuestName = reservation.GuestName,
            Lines = new List<InvoiceLine>
            {
                new() { Description = $"{reservation.Nights} night(s) × {nightRate:C}/night", Amount = subtotal },
                new() { Description = "TVA (10%)", Amount = tva },
                new() { Description = $"Tourist tax ({reservation.GuestCount} pers. × {reservation.Nights} nights × 1.50€)", Amount = touristTax }
            }
        };
    }
}
