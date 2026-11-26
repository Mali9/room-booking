using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>Lists, quotes and places Furniture orders.</summary>
public sealed class FurnitureService(
    IFurnitureRepository repository,
    FurnitureValidator validator,
    FurniturePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<FurnitureItem> ListItems() => repository.Items();

    public (FurnitureItem Item, FurnitureQuote Quote) Quote(string itemId, int quantity)
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
        var order = new FurnitureOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Furniture order {order.Id} by {customerId}");
        return FurnitureMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<FurnitureOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public FurnitureItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
