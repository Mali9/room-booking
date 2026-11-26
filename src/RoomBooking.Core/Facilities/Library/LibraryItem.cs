namespace RoomBooking.Core.Facilities.Library;

/// <summary>Something customers can order from Library.</summary>
public sealed class LibraryItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
