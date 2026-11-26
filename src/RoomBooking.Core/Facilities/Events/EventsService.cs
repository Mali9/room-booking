using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Events;

/// <summary>Lists, quotes and places Events orders.</summary>
public sealed class EventsService(
    IEventsRepository repository,
    EventsValidator validator,
    EventsPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<EventsItem> ListItems() => repository.Items();

    public (EventsItem Item, EventsQuote Quote) Quote(string itemId, int quantity)
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
        var order = new EventsOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Events order {order.Id} by {customerId}");
        return EventsMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<EventsOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public EventsItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
