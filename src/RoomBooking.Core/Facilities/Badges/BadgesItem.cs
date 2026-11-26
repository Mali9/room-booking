namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Something customers can order from Badges.</summary>
public sealed class BadgesItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
