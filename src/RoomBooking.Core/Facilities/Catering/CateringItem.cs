namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Something customers can order from Catering.</summary>
public sealed class CateringItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
