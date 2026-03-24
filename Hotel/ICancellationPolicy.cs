namespace Hotel;

public interface ICancellationPolicy
{
    bool CanCancel(Reservation reservation);
    decimal CalculateRefund(Reservation reservation, decimal totalPrice);
}

public class FlexibleCancellationPolicy : ICancellationPolicy
{
    public bool CanCancel(Reservation reservation) => reservation.Status != "CheckedIn";
    public decimal CalculateRefund(Reservation reservation, decimal totalPrice)
    {
        var daysBeforeCheckIn = (reservation.CheckIn - DateTime.Now).Days;
        return daysBeforeCheckIn >= 1 ? totalPrice : 0m;
    }
}

public class ModerateCancellationPolicy : ICancellationPolicy
{
    public bool CanCancel(Reservation reservation) => reservation.Status != "CheckedIn";
    public decimal CalculateRefund(Reservation reservation, decimal totalPrice)
    {
        var daysBeforeCheckIn = (reservation.CheckIn - DateTime.Now).Days;
        if (daysBeforeCheckIn >= 5) return totalPrice;
        if (daysBeforeCheckIn >= 2) return totalPrice * 0.5m;
        return 0m;
    }
}

public class StrictCancellationPolicy : ICancellationPolicy
{
    public bool CanCancel(Reservation reservation) => reservation.Status == "Confirmed";
    public decimal CalculateRefund(Reservation reservation, decimal totalPrice)
    {
        var daysBeforeCheckIn = (reservation.CheckIn - DateTime.Now).Days;
        if (daysBeforeCheckIn >= 14) return totalPrice;
        if (daysBeforeCheckIn >= 7) return totalPrice * 0.5m;
        return 0m;
    }
}

public class NonRefundableCancellationPolicy : ICancellationPolicy
{
    public bool CanCancel(Reservation reservation) => false;
    public decimal CalculateRefund(Reservation reservation, decimal totalPrice) => 0m;
}
