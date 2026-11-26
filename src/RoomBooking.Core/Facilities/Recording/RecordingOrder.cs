namespace RoomBooking.Core.Facilities.Recording;

/// <summary>One Recording order placed by a customer.</summary>
public sealed class RecordingOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
