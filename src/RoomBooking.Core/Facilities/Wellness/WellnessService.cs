using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Lists, quotes and places Wellness orders.</summary>
public sealed class WellnessService(
    IWellnessRepository repository,
    WellnessValidator validator,
    WellnessPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<WellnessItem> ListItems() => repository.Items();

    public (WellnessItem Item, WellnessQuote Quote) Quote(string itemId, int quantity)
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
        var order = new WellnessOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Wellness order {order.Id} by {customerId}");
        return WellnessMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<WellnessOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public WellnessItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
