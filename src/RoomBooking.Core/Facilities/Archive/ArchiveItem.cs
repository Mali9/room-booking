namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Something customers can order from Archive.</summary>
public sealed class ArchiveItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
