namespace RoomBooking.Core.Facilities.Recording;

/// <summary>The price of an order before it is placed.</summary>
public sealed record RecordingQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
