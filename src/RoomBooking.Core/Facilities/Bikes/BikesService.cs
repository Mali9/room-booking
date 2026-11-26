using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>Lists, quotes and places Bikes orders.</summary>
public sealed class BikesService(
    IBikesRepository repository,
    BikesValidator validator,
    BikesPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<BikesItem> ListItems() => repository.Items();

    public (BikesItem Item, BikesQuote Quote) Quote(string itemId, int quantity)
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
        var order = new BikesOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Bikes order {order.Id} by {customerId}");
        return BikesMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<BikesOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public BikesItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
