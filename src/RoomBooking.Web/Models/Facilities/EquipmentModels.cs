using RoomBooking.Core.Facilities.Equipment;

namespace RoomBooking.Web.Models.Facilities;

public sealed class EquipmentQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public EquipmentItem? Item { get; set; }
    public EquipmentQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class EquipmentReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
