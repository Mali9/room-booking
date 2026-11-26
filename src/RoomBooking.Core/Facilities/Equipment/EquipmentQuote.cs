namespace RoomBooking.Core.Facilities.Equipment;

/// <summary>The price of an order before it is placed.</summary>
public sealed record EquipmentQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
