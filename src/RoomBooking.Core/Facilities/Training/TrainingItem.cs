namespace RoomBooking.Core.Facilities.Training;

/// <summary>Something customers can order from Training.</summary>
public sealed class TrainingItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
