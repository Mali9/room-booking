using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Garden;

/// <summary>Lists, quotes and places Garden orders.</summary>
public sealed class GardenService(
    IGardenRepository repository,
    GardenValidator validator,
    GardenPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<GardenItem> ListItems() => repository.Items();

    public (GardenItem Item, GardenQuote Quote) Quote(string itemId, int quantity)
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
        var order = new GardenOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Garden order {order.Id} by {customerId}");
        return GardenMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<GardenOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public GardenItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
