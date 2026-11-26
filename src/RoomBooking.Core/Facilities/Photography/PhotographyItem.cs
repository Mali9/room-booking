namespace RoomBooking.Core.Facilities.Photography;

/// <summary>Something customers can order from Photography.</summary>
public sealed class PhotographyItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
