namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>Something customers can order from Furniture.</summary>
public sealed class FurnitureItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
