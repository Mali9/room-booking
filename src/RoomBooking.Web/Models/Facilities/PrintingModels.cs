using RoomBooking.Core.Facilities.Printing;

namespace RoomBooking.Web.Models.Facilities;

public sealed class PrintingQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public PrintingItem? Item { get; set; }
    public PrintingQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class PrintingReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
