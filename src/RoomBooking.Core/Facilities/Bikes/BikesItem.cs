namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>Something customers can order from Bikes.</summary>
public sealed class BikesItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
