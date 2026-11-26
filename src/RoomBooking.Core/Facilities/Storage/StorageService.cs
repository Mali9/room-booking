using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Storage;

/// <summary>Lists, quotes and places Storage orders.</summary>
public sealed class StorageService(
    IStorageRepository repository,
    StorageValidator validator,
    StoragePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<StorageItem> ListItems() => repository.Items();

    public (StorageItem Item, StorageQuote Quote) Quote(string itemId, int quantity)
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
        var order = new StorageOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Storage order {order.Id} by {customerId}");
        return StorageMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<StorageOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public StorageItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
