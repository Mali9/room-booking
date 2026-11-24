namespace RoomBooking.Core.Domain;

public enum InvoiceKind
{
    Booking,
    CancellationFee,
}

public enum InvoiceStatus
{
    Issued,
    Void,
}

public sealed record InvoiceLine(string Description, decimal Amount);

public sealed class Invoice
{
    public required string Id { get; init; }
    public required string BookingId { get; init; }
    public required string CustomerId { get; init; }
    public InvoiceKind Kind { get; init; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Issued;
    public DateTime IssuedAt { get; init; }
    public List<InvoiceLine> Lines { get; init; } = [];
    public decimal Subtotal { get; init; }
    public decimal Vat { get; init; }

    public decimal Total => Subtotal + Vat;
}
