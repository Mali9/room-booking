namespace RoomBooking.Core.Facilities.Storage;

/// <summary>Something customers can order from Storage.</summary>
public sealed class StorageItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
