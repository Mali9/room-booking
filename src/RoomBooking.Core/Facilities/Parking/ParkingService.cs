using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Parking;

/// <summary>Lists, quotes and places Parking orders.</summary>
public sealed class ParkingService(
    IParkingRepository repository,
    ParkingValidator validator,
    ParkingPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ParkingItem> ListItems() => repository.Items();

    public (ParkingItem Item, ParkingQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ParkingOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Parking order {order.Id} by {customerId}");
        return ParkingMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ParkingOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ParkingItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
