namespace RoomBooking.Core.Facilities.Concierge;

/// <summary>Something customers can order from Concierge.</summary>
public sealed class ConciergeItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
