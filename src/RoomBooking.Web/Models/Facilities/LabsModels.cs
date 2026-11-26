using RoomBooking.Core.Facilities.Labs;

namespace RoomBooking.Web.Models.Facilities;

public sealed class LabsQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public LabsItem? Item { get; set; }
    public LabsQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class LabsReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
