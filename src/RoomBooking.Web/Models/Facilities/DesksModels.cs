using RoomBooking.Core.Facilities.Desks;

namespace RoomBooking.Web.Models.Facilities;

public sealed class DesksQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public DesksItem? Item { get; set; }
    public DesksQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class DesksReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
