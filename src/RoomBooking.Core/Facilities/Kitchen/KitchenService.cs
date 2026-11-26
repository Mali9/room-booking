using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Lists, quotes and places Kitchen orders.</summary>
public sealed class KitchenService(
    IKitchenRepository repository,
    KitchenValidator validator,
    KitchenPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<KitchenItem> ListItems() => repository.Items();

    public (KitchenItem Item, KitchenQuote Quote) Quote(string itemId, int quantity)
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
        var order = new KitchenOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Kitchen order {order.Id} by {customerId}");
        return KitchenMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<KitchenOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public KitchenItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
