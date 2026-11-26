using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Printing;

/// <summary>Lists, quotes and places Printing orders.</summary>
public sealed class PrintingService(
    IPrintingRepository repository,
    PrintingValidator validator,
    PrintingPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<PrintingItem> ListItems() => repository.Items();

    public (PrintingItem Item, PrintingQuote Quote) Quote(string itemId, int quantity)
    {
        var item = Find(itemId);
        validator.Validate(item, quantity);
        return (item, pricing.Quote(item, quantity));
    }

    public string Place(string customerId, string itemId, int quantity)
    {
        var item = Find(itemId);
        validator.Validate(item, quantity);
        var quote = pricing.Quote(item, quantity);
        var order = new PrintingOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Printing order {order.Id} by {customerId}");
        return PrintingMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<PrintingOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public PrintingItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
