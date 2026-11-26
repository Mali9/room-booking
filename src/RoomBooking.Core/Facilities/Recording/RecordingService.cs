using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Recording;

/// <summary>Lists, quotes and places Recording orders.</summary>
public sealed class RecordingService(
    IRecordingRepository repository,
    RecordingValidator validator,
    RecordingPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<RecordingItem> ListItems() => repository.Items();

    public (RecordingItem Item, RecordingQuote Quote) Quote(string itemId, int quantity)
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
        var order = new RecordingOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Recording order {order.Id} by {customerId}");
        return RecordingMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<RecordingOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public RecordingItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
