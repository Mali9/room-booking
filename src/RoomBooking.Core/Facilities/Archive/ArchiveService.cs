using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Lists, quotes and places Archive orders.</summary>
public sealed class ArchiveService(
    IArchiveRepository repository,
    ArchiveValidator validator,
    ArchivePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ArchiveItem> ListItems() => repository.Items();

    public (ArchiveItem Item, ArchiveQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ArchiveOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Archive order {order.Id} by {customerId}");
        return ArchiveMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ArchiveOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ArchiveItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
