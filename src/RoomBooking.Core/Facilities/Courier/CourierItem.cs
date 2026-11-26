namespace RoomBooking.Core.Facilities.Courier;

/// <summary>Something customers can order from Courier.</summary>
public sealed class CourierItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
