using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>Lists, quotes and places Lounge orders.</summary>
public sealed class LoungeService(
    ILoungeRepository repository,
    LoungeValidator validator,
    LoungePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<LoungeItem> ListItems() => repository.Items();

    public (LoungeItem Item, LoungeQuote Quote) Quote(string itemId, int quantity)
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
        var order = new LoungeOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Lounge order {order.Id} by {customerId}");
        return LoungeMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<LoungeOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public LoungeItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
