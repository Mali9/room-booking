namespace RoomBooking.Core.Facilities.Keys;

/// <summary>Something customers can order from Keys.</summary>
public sealed class KeysItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
