namespace RoomBooking.Core.Facilities.Parking;

/// <summary>One Parking order placed by a customer.</summary>
public sealed class ParkingOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
