namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Something customers can order from Wellness.</summary>
public sealed class WellnessItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
