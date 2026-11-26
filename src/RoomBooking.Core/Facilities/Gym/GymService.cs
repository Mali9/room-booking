using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Gym;

/// <summary>Lists, quotes and places Gym orders.</summary>
public sealed class GymService(
    IGymRepository repository,
    GymValidator validator,
    GymPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<GymItem> ListItems() => repository.Items();

    public (GymItem Item, GymQuote Quote) Quote(string itemId, int quantity)
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
        var order = new GymOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Gym order {order.Id} by {customerId}");
        return GymMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<GymOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public GymItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
