using RoomBooking.Core.Facilities.Laundry;

namespace RoomBooking.Web.Models.Facilities;

public sealed class LaundryQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public LaundryItem? Item { get; set; }
    public LaundryQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class LaundryReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
