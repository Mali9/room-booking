namespace RoomBooking.Core.Facilities.Events;

/// <summary>Something customers can order from Events.</summary>
public sealed class EventsItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
