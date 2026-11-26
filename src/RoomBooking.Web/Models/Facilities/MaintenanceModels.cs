using RoomBooking.Core.Facilities.Maintenance;

namespace RoomBooking.Web.Models.Facilities;

public sealed class MaintenanceQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public MaintenanceItem? Item { get; set; }
    public MaintenanceQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class MaintenanceReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
