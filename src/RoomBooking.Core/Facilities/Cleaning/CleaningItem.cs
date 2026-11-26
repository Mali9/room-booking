namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>Something customers can order from Cleaning.</summary>
public sealed class CleaningItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
