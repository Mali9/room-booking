namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Something customers can order from Mailroom.</summary>
public sealed class MailroomItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
