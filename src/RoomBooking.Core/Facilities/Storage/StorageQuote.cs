namespace RoomBooking.Core.Facilities.Storage;

/// <summary>The price of an order before it is placed.</summary>
public sealed record StorageQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
