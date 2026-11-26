namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Something customers can order from Lockers.</summary>
public sealed class LockersItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
