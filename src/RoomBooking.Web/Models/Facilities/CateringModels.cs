using RoomBooking.Core.Facilities.Catering;

namespace RoomBooking.Web.Models.Facilities;

public sealed class CateringQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public CateringItem? Item { get; set; }
    public CateringQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class CateringReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
