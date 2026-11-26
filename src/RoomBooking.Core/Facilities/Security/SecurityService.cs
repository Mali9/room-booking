using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Security;

/// <summary>Lists, quotes and places Security orders.</summary>
public sealed class SecurityService(
    ISecurityRepository repository,
    SecurityValidator validator,
    SecurityPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<SecurityItem> ListItems() => repository.Items();

    public (SecurityItem Item, SecurityQuote Quote) Quote(string itemId, int quantity)
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
        var order = new SecurityOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Security order {order.Id} by {customerId}");
        return SecurityMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<SecurityOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public SecurityItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
