namespace RoomBooking.Core.Facilities.Security;

/// <summary>Something customers can order from Security.</summary>
public sealed class SecurityItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
