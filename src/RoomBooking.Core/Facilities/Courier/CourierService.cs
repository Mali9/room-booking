using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Courier;

/// <summary>Lists, quotes and places Courier orders.</summary>
public sealed class CourierService(
    ICourierRepository repository,
    CourierValidator validator,
    CourierPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<CourierItem> ListItems() => repository.Items();

    public (CourierItem Item, CourierQuote Quote) Quote(string itemId, int quantity)
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
        var order = new CourierOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Courier order {order.Id} by {customerId}");
        return CourierMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<CourierOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public CourierItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
