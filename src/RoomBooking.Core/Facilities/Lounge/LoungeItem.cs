namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>Something customers can order from Lounge.</summary>
public sealed class LoungeItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
