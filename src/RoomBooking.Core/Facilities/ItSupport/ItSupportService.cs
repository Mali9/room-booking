using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>Lists, quotes and places ItSupport orders.</summary>
public sealed class ItSupportService(
    IItSupportRepository repository,
    ItSupportValidator validator,
    ItSupportPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ItSupportItem> ListItems() => repository.Items();

    public (ItSupportItem Item, ItSupportQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ItSupportOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"ItSupport order {order.Id} by {customerId}");
        return ItSupportMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ItSupportOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ItSupportItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
