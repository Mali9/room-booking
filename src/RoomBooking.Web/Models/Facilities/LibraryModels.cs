using RoomBooking.Core.Facilities.Library;

namespace RoomBooking.Web.Models.Facilities;

public sealed class LibraryQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public LibraryItem? Item { get; set; }
    public LibraryQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class LibraryReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
