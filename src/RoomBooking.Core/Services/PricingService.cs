using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

public sealed record PriceQuote(int PeakHours, int OffPeakHours, decimal Gross, decimal Discount)
{
    /// <summary>Price after discount, excluding VAT.</summary>
    public decimal Total => Gross - Discount;
}

public sealed class PricingService(PricingPolicy policy)
{
    public PriceQuote Quote(Room room, DateTime start, int hours)
    {
        var peakHours = 0;
        var offPeakHours = 0;
        for (var hour = 0; hour < hours; hour++)
        {
            if (IsPeak(start.AddHours(hour)))
            {
                peakHours++;
            }
            else
            {
                offPeakHours++;
            }
        }

        var gross = peakHours * room.HourlyRate * policy.PeakMultiplier
            + offPeakHours * room.HourlyRate;
        var discount = hours >= policy.LongBookingHours
            ? Round(gross * policy.LongBookingDiscountRate)
            : 0m;
        return new PriceQuote(peakHours, offPeakHours, Round(gross), discount);
    }

    public bool IsPeak(DateTime slotStart) =>
        policy.PeakDays.Contains(slotStart.DayOfWeek)
        && slotStart.TimeOfDay >= policy.PeakStart
        && slotStart.TimeOfDay < policy.PeakEnd;

    public decimal CalculateVat(decimal amount) => Round(amount * policy.VatRate);

    public decimal VatRate => policy.VatRate;

    public decimal LongBookingDiscountRate => policy.LongBookingDiscountRate;

    private static decimal Round(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
