using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Lists, quotes and places Catering orders.</summary>
public sealed class CateringService(
    ICateringRepository repository,
    CateringValidator validator,
    CateringPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<CateringItem> ListItems() => repository.Items();

    public (CateringItem Item, CateringQuote Quote) Quote(string itemId, int quantity)
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
        var order = new CateringOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Catering order {order.Id} by {customerId}");
        return CateringMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<CateringOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public CateringItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
