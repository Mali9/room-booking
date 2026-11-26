using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>Lists, quotes and places Shuttle orders.</summary>
public sealed class ShuttleService(
    IShuttleRepository repository,
    ShuttleValidator validator,
    ShuttlePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ShuttleItem> ListItems() => repository.Items();

    public (ShuttleItem Item, ShuttleQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ShuttleOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Shuttle order {order.Id} by {customerId}");
        return ShuttleMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ShuttleOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ShuttleItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
