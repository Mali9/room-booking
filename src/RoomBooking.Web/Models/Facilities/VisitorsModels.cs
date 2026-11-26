using RoomBooking.Core.Facilities.Visitors;

namespace RoomBooking.Web.Models.Facilities;

public sealed class VisitorsQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public VisitorsItem? Item { get; set; }
    public VisitorsQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class VisitorsReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
