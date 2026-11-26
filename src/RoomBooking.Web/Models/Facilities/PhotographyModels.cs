using RoomBooking.Core.Facilities.Photography;

namespace RoomBooking.Web.Models.Facilities;

public sealed class PhotographyQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public PhotographyItem? Item { get; set; }
    public PhotographyQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class PhotographyReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
