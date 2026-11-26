namespace RoomBooking.Core.Facilities.Parking;

/// <summary>Something customers can order from Parking.</summary>
public sealed class ParkingItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
