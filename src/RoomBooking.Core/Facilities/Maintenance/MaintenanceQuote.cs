namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>The price of an order before it is placed.</summary>
public sealed record MaintenanceQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
