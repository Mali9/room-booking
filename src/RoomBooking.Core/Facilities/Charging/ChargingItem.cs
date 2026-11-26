namespace RoomBooking.Core.Facilities.Charging;

/// <summary>Something customers can order from Charging.</summary>
public sealed class ChargingItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
