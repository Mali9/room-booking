namespace RoomBooking.Core.Facilities.Badges;

/// <summary>The price of an order before it is placed.</summary>
public sealed record BadgesQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
