namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ChildcareQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
