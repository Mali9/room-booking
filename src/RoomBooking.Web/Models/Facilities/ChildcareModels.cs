using RoomBooking.Core.Facilities.Childcare;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ChildcareQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ChildcareItem? Item { get; set; }
    public ChildcareQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ChildcareReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
