namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Something customers can order from Studio.</summary>
public sealed class StudioItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
