using RoomBooking.Core.Facilities.Security;

namespace RoomBooking.Web.Models.Facilities;

public sealed class SecurityQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public SecurityItem? Item { get; set; }
    public SecurityQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class SecurityReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
