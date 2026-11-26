using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Keys;

/// <summary>Lists, quotes and places Keys orders.</summary>
public sealed class KeysService(
    IKeysRepository repository,
    KeysValidator validator,
    KeysPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<KeysItem> ListItems() => repository.Items();

    public (KeysItem Item, KeysQuote Quote) Quote(string itemId, int quantity)
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
        var order = new KeysOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Keys order {order.Id} by {customerId}");
        return KeysMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<KeysOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public KeysItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
