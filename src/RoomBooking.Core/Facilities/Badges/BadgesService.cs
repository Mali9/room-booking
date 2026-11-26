using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Lists, quotes and places Badges orders.</summary>
public sealed class BadgesService(
    IBadgesRepository repository,
    BadgesValidator validator,
    BadgesPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<BadgesItem> ListItems() => repository.Items();

    public (BadgesItem Item, BadgesQuote Quote) Quote(string itemId, int quantity)
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
        var order = new BadgesOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Badges order {order.Id} by {customerId}");
        return BadgesMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<BadgesOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public BadgesItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
