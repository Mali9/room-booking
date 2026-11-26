using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Lists, quotes and places Lockers orders.</summary>
public sealed class LockersService(
    ILockersRepository repository,
    LockersValidator validator,
    LockersPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<LockersItem> ListItems() => repository.Items();

    public (LockersItem Item, LockersQuote Quote) Quote(string itemId, int quantity)
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
        var order = new LockersOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Lockers order {order.Id} by {customerId}");
        return LockersMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<LockersOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public LockersItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
