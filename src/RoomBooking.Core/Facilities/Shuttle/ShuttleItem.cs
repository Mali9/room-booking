namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>Something customers can order from Shuttle.</summary>
public sealed class ShuttleItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
