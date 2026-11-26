namespace RoomBooking.Core.Facilities.Equipment;

/// <summary>Something customers can order from Equipment.</summary>
public sealed class EquipmentItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
