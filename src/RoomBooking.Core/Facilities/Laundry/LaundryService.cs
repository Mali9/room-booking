using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>Lists, quotes and places Laundry orders.</summary>
public sealed class LaundryService(
    ILaundryRepository repository,
    LaundryValidator validator,
    LaundryPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<LaundryItem> ListItems() => repository.Items();

    public (LaundryItem Item, LaundryQuote Quote) Quote(string itemId, int quantity)
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
        var order = new LaundryOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Laundry order {order.Id} by {customerId}");
        return LaundryMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<LaundryOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public LaundryItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
