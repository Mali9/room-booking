namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>The price of an order before it is placed.</summary>
public sealed record WellnessQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
