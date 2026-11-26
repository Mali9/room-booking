namespace RoomBooking.Core.Facilities.Visitors;

/// <summary>Something customers can order from Visitors.</summary>
public sealed class VisitorsItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
