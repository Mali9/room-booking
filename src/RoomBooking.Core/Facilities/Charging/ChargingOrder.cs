namespace RoomBooking.Core.Facilities.Charging;

/// <summary>One Charging order placed by a customer.</summary>
public sealed class ChargingOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
