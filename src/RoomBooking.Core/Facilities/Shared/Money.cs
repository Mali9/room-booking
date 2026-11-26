namespace RoomBooking.Core.Facilities.Shared;

/// <summary>Money rounding shared by every facility: two decimals, half away from zero.</summary>
public static class FacilityMoney
{
    public static decimal Round(decimal amount) =>
        Math.Round(amount, 2, MidpointRounding.AwayFromZero);
}
