using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Visitors;

/// <summary>Lists, quotes and places Visitors orders.</summary>
public sealed class VisitorsService(
    IVisitorsRepository repository,
    VisitorsValidator validator,
    VisitorsPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<VisitorsItem> ListItems() => repository.Items();

    public (VisitorsItem Item, VisitorsQuote Quote) Quote(string itemId, int quantity)
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
        var order = new VisitorsOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Visitors order {order.Id} by {customerId}");
        return VisitorsMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<VisitorsOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public VisitorsItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
