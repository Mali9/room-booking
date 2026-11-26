using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>Lists, quotes and places Cleaning orders.</summary>
public sealed class CleaningService(
    ICleaningRepository repository,
    CleaningValidator validator,
    CleaningPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<CleaningItem> ListItems() => repository.Items();

    public (CleaningItem Item, CleaningQuote Quote) Quote(string itemId, int quantity)
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
        var order = new CleaningOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Cleaning order {order.Id} by {customerId}");
        return CleaningMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<CleaningOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public CleaningItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
