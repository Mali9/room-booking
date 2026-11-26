namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Something customers can order from Translation.</summary>
public sealed class TranslationItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
