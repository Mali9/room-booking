namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>One Laundry order placed by a customer.</summary>
public sealed class LaundryOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
