using RoomBooking.Core.Facilities.Uniforms;

namespace RoomBooking.Web.Models.Facilities;

public sealed class UniformsQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public UniformsItem? Item { get; set; }
    public UniformsQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class UniformsReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
