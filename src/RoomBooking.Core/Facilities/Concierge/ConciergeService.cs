using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Concierge;

/// <summary>Lists, quotes and places Concierge orders.</summary>
public sealed class ConciergeService(
    IConciergeRepository repository,
    ConciergeValidator validator,
    ConciergePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ConciergeItem> ListItems() => repository.Items();

    public (ConciergeItem Item, ConciergeQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ConciergeOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Concierge order {order.Id} by {customerId}");
        return ConciergeMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ConciergeOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ConciergeItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
