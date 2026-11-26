namespace RoomBooking.Core.Facilities.Recording;

/// <summary>Something customers can order from Recording.</summary>
public sealed class RecordingItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
