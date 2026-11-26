namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>The price of an order before it is placed.</summary>
public sealed record CleaningQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
