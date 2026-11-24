using System.Globalization;
using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>Invoices with VAT (BRD BR-12 and BR-15).</summary>
public sealed class InvoiceService(
    IInvoiceRepository invoices,
    PricingService pricing,
    IClock clock)
{
    public Invoice IssueForBooking(Booking booking, Room room, PriceQuote quote)
    {
        var lines = new List<InvoiceLine>
        {
            new(
                $"{room.Name}, {booking.Hours} h ({quote.PeakHours} peak, {quote.OffPeakHours} off-peak)",
                quote.Gross),
        };
        if (quote.Discount > 0)
        {
            var percent = (pricing.LongBookingDiscountRate * 100).ToString("0", CultureInfo.InvariantCulture);
            lines.Add(new InvoiceLine($"Long booking discount ({percent}%)", -quote.Discount));
        }

        return Issue(booking, InvoiceKind.Booking, lines, quote.Total);
    }

    public Invoice IssueCancellationFee(Booking booking, decimal fee)
    {
        var lines = new List<InvoiceLine>
        {
            new($"Late cancellation fee for {booking.Id}", fee),
        };
        return Issue(booking, InvoiceKind.CancellationFee, lines, fee);
    }

    public void VoidBookingInvoices(string bookingId)
    {
        foreach (var invoice in invoices.GetByBooking(bookingId))
        {
            if (invoice.Kind == InvoiceKind.Booking && invoice.Status == InvoiceStatus.Issued)
            {
                invoice.Status = InvoiceStatus.Void;
                invoices.Update(invoice);
            }
        }
    }

    public IReadOnlyList<Invoice> GetVisibleTo(string userId, bool isAdmin)
    {
        var visible = isAdmin ? invoices.GetAll() : invoices.GetByCustomer(userId);
        return visible.OrderBy(invoice => invoice.Id, StringComparer.Ordinal).ToList();
    }

    public Invoice Get(string invoiceId, string userId, bool isAdmin)
    {
        var invoice = invoices.GetById(invoiceId);
        if (invoice is null || (!isAdmin && invoice.CustomerId != userId))
        {
            throw new BusinessRuleException(ErrorCodes.NotFound, "Invoice not found.");
        }

        return invoice;
    }

    private Invoice Issue(Booking booking, InvoiceKind kind, List<InvoiceLine> lines, decimal subtotal)
    {
        var invoice = new Invoice
        {
            Id = invoices.NextId(),
            BookingId = booking.Id,
            CustomerId = booking.CustomerId,
            Kind = kind,
            IssuedAt = clock.Now,
            Lines = lines,
            Subtotal = subtotal,
            Vat = pricing.CalculateVat(subtotal),
        };
        invoices.Add(invoice);
        return invoice;
    }
}
