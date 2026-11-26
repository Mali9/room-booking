namespace RoomBooking.Core.Facilities.Pantry;

/// <summary>Something customers can order from Pantry.</summary>
public sealed class PantryItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
