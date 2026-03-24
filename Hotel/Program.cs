using Hotel;

Console.WriteLine("=== Le Mas des Oliviers — Hotel Management ===\n");

// --- Setup ---
var rooms = new List<Room>
{
    new() { Id = "101", Type = RoomType.Standard, Capacity = 2, BasePrice = 80m },
    new() { Id = "102", Type = RoomType.Standard, Capacity = 2, BasePrice = 80m },
    new() { Id = "201", Type = RoomType.Suite, Capacity = 2, BasePrice = 200m },
    new() { Id = "301", Type = RoomType.Family, Capacity = 4, BasePrice = 120m },
};

var reservationStore = new InMemoryReservationStore();
var roomStore = new InMemoryRoomStore(rooms);
var emailSender = new EmailSender();
var smsSender = new SmsSender();
var roomAssigner = new RoomAssigner(roomStore, reservationStore);
var pricingFactory = new PricingStrategyFactory();
var taxCalculator = new TaxCalculator();
var invoiceGenerator = new InvoiceGenerator(pricingFactory, taxCalculator, roomStore);

var bookingService = new BookingService(reservationStore, roomAssigner, emailSender);
var billingService = new BillingService(invoiceGenerator, reservationStore);
var housekeepingScheduler = new HousekeepingScheduler(
    reservationStore, new StandardCleaningPolicy(), smsSender);

// --- Scenario 1: Create reservations ---
Console.WriteLine("--- Creating reservations ---\n");

var alice = bookingService.CreateReservation(
    "Alice Martin", RoomType.Standard,
    new DateTime(2025, 6, 15), new DateTime(2025, 6, 18),
    2, "alice@example.com", "+33612345001");

Console.WriteLine();

var bob = bookingService.CreateReservation(
    "Bob Dupont", RoomType.Suite,
    new DateTime(2025, 6, 15), new DateTime(2025, 6, 22),
    2, "bob@example.com", "+33612345002");

Console.WriteLine();

var durand = bookingService.CreateReservation(
    "Famille Durand", RoomType.Family,
    new DateTime(2025, 6, 20), new DateTime(2025, 6, 25),
    4, "durand@example.com", "+33612345003");

// --- Scenario 2: Conflict (overlap) ---
Console.WriteLine("\n--- Attempting conflicting reservation ---\n");
try
{
    bookingService.CreateReservation(
        "Charlie Noir", RoomType.Standard,
        new DateTime(2025, 6, 16), new DateTime(2025, 6, 19),
        2, "charlie@example.com", "+33612345004");
}
catch (Exception ex)
{
    Console.WriteLine($"  Expected conflict: {ex.Message}");
}

// --- Scenario 3: Check-in ---
Console.WriteLine("\n--- Check-in ---\n");
bookingService.CheckIn(alice.Id);

// --- Scenario 4: Invoice ---
Console.WriteLine("\n--- Invoice for Bob ---\n");
var invoice = billingService.GetInvoice(bob.Id);
invoice.Print();

// --- Scenario 5: Housekeeping schedule ---
Console.WriteLine("\n--- Housekeeping schedule for June 18 ---\n");
var schedule = housekeepingScheduler.GetSchedule(new DateTime(2025, 6, 18));
if (schedule.Count == 0)
    Console.WriteLine("  No cleaning tasks for this date.");
else
    foreach (var task in schedule)
        Console.WriteLine($"  [{task.Type}] Room {task.RoomId} (Reservation: {task.ReservationId})");

// --- Scenario 6: Check-out ---
Console.WriteLine("\n--- Check-out ---\n");
bookingService.CheckOut(alice.Id);

Console.WriteLine("\n=== Done ===");
