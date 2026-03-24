namespace Hotel;

public class BillingService
{
    private readonly InvoiceGenerator _invoiceGenerator;
    private readonly IReservationRepository _reservationRepo;

    public BillingService(
        InvoiceGenerator invoiceGenerator,
        IReservationRepository reservationRepo)
    {
        _invoiceGenerator = invoiceGenerator;
        _reservationRepo = reservationRepo;
    }

    public Invoice GetInvoice(string reservationId)
    {
        var reservation = _reservationRepo.GetById(reservationId)
            ?? throw new Exception($"Reservation {reservationId} not found");

        return _invoiceGenerator.Generate(reservation);
    }
}
