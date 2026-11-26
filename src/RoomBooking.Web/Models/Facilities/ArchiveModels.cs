using RoomBooking.Core.Facilities.Archive;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ArchiveQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ArchiveItem? Item { get; set; }
    public ArchiveQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ArchiveReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
