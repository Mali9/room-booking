namespace RoomBooking.Core.Facilities.Garden;

/// <summary>Something customers can order from Garden.</summary>
public sealed class GardenItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
