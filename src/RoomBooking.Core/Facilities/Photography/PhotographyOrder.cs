namespace RoomBooking.Core.Facilities.Photography;

/// <summary>One Photography order placed by a customer.</summary>
public sealed class PhotographyOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
