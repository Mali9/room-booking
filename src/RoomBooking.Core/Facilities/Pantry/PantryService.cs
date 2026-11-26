using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Pantry;

/// <summary>Lists, quotes and places Pantry orders.</summary>
public sealed class PantryService(
    IPantryRepository repository,
    PantryValidator validator,
    PantryPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<PantryItem> ListItems() => repository.Items();

    public (PantryItem Item, PantryQuote Quote) Quote(string itemId, int quantity)
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
        var order = new PantryOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Pantry order {order.Id} by {customerId}");
        return PantryMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<PantryOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public PantryItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
