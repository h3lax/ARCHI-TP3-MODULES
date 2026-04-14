using Hotel.Booking.Contracts;

namespace Hotel.Billing.Pricing;

public interface IPricingStrategy
{
    decimal CalculateNightRate(Room room);
}

public class StandardPricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(Room room) => room.BasePrice;
}

public class SuitePricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(Room room) => room.BasePrice * 1.2m;
}

public class FamilyPricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(Room room) => room.BasePrice * 0.9m;
}

public class PricingStrategyFactory
{
    public IPricingStrategy Create(RoomType roomType) => roomType switch
    {
        RoomType.Standard => new StandardPricingStrategy(),
        RoomType.Suite => new SuitePricingStrategy(),
        RoomType.Family => new FamilyPricingStrategy(),
        _ => new StandardPricingStrategy()
    };
}
