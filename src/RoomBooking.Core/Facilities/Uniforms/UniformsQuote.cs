namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>The price of an order before it is placed.</summary>
public sealed record UniformsQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
