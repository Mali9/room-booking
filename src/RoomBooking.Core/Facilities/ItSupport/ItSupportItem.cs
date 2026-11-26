namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>Something customers can order from ItSupport.</summary>
public sealed class ItSupportItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
