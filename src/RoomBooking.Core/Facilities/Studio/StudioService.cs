using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Lists, quotes and places Studio orders.</summary>
public sealed class StudioService(
    IStudioRepository repository,
    StudioValidator validator,
    StudioPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<StudioItem> ListItems() => repository.Items();

    public (StudioItem Item, StudioQuote Quote) Quote(string itemId, int quantity)
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
        var order = new StudioOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Studio order {order.Id} by {customerId}");
        return StudioMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<StudioOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public StudioItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
