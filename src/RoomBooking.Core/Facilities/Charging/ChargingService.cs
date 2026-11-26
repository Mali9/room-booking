using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Charging;

/// <summary>Lists, quotes and places Charging orders.</summary>
public sealed class ChargingService(
    IChargingRepository repository,
    ChargingValidator validator,
    ChargingPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ChargingItem> ListItems() => repository.Items();

    public (ChargingItem Item, ChargingQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ChargingOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Charging order {order.Id} by {customerId}");
        return ChargingMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ChargingOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ChargingItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
