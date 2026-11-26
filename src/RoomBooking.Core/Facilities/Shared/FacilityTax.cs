namespace RoomBooking.Core.Facilities.Shared;

/// <summary>VAT added to every facility price. Every facility pricing class uses it.</summary>
public sealed class FacilityTax
{
    public decimal VatRate { get; init; } = 0.16m;

    public decimal Apply(decimal subtotal) => FacilityMoney.Round(subtotal * (1 + VatRate));
}
