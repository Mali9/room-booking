namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>The price of an order before it is placed.</summary>
public sealed record MailroomQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
