namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>One Furniture order placed by a customer.</summary>
public sealed class FurnitureOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
