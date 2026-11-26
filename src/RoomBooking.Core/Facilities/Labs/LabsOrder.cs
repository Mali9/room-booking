namespace RoomBooking.Core.Facilities.Labs;

/// <summary>One Labs order placed by a customer.</summary>
public sealed class LabsOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
