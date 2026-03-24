namespace Hotel;

public class Invoice
{
    public string ReservationId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public List<InvoiceLine> Lines { get; set; } = new();
    public decimal Total => Lines.Sum(l => l.Amount);

    public void Print()
    {
        Console.WriteLine($"  Invoice for {GuestName} (Reservation: {ReservationId})");
        foreach (var line in Lines)
            Console.WriteLine($"    {line.Description,-40} {line.Amount,10:C}");
        Console.WriteLine($"    {"TOTAL",-40} {Total,10:C}");
    }
}
