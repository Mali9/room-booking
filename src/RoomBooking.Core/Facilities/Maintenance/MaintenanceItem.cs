namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>Something customers can order from Maintenance.</summary>
public sealed class MaintenanceItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
