namespace RoomBooking.Core.Facilities.Archive;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ArchiveQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
