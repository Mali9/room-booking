namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>Something customers can order from Laundry.</summary>
public sealed class LaundryItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
