using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Desks;

/// <summary>Lists, quotes and places Desks orders.</summary>
public sealed class DesksService(
    IDesksRepository repository,
    DesksValidator validator,
    DesksPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<DesksItem> ListItems() => repository.Items();

    public (DesksItem Item, DesksQuote Quote) Quote(string itemId, int quantity)
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
        var order = new DesksOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Desks order {order.Id} by {customerId}");
        return DesksMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<DesksOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public DesksItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
