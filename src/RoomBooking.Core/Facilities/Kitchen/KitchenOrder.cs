namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>One Kitchen order placed by a customer.</summary>
public sealed class KitchenOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
