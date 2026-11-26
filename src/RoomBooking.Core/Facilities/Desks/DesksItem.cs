namespace RoomBooking.Core.Facilities.Desks;

/// <summary>Something customers can order from Desks.</summary>
public sealed class DesksItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
