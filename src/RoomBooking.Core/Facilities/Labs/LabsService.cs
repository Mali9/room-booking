using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Labs;

/// <summary>Lists, quotes and places Labs orders.</summary>
public sealed class LabsService(
    ILabsRepository repository,
    LabsValidator validator,
    LabsPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<LabsItem> ListItems() => repository.Items();

    public (LabsItem Item, LabsQuote Quote) Quote(string itemId, int quantity)
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
        var order = new LabsOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Labs order {order.Id} by {customerId}");
        return LabsMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<LabsOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public LabsItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
