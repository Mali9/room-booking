namespace RoomBooking.Core.Facilities.Signage;

/// <summary>Something customers can order from Signage.</summary>
public sealed class SignageItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
