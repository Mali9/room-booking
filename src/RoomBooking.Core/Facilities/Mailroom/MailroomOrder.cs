namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>One Mailroom order placed by a customer.</summary>
public sealed class MailroomOrder
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
