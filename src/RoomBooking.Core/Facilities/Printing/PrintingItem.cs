namespace RoomBooking.Core.Facilities.Printing;

/// <summary>Something customers can order from Printing.</summary>
public sealed class PrintingItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
