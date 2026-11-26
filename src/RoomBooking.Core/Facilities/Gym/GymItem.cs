namespace RoomBooking.Core.Facilities.Gym;

/// <summary>Something customers can order from Gym.</summary>
public sealed class GymItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
