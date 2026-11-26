namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Something customers can order from Childcare.</summary>
public sealed class ChildcareItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
