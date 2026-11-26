namespace RoomBooking.Core.Facilities.Catering;

/// <summary>One Catering order placed by a customer.</summary>
public sealed class CateringOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
