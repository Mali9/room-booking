namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Something customers can order from Kitchen.</summary>
public sealed class KitchenItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
