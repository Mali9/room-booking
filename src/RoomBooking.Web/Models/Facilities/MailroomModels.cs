using RoomBooking.Core.Facilities.Mailroom;

namespace RoomBooking.Web.Models.Facilities;

public sealed class MailroomQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public MailroomItem? Item { get; set; }
    public MailroomQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class MailroomReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
