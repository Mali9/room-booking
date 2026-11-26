namespace RoomBooking.Core.Facilities.Labs;

/// <summary>Something customers can order from Labs.</summary>
public sealed class LabsItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
