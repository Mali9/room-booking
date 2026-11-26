using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Equipment;

/// <summary>Lists, quotes and places Equipment orders.</summary>
public sealed class EquipmentService(
    IEquipmentRepository repository,
    EquipmentValidator validator,
    EquipmentPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<EquipmentItem> ListItems() => repository.Items();

    public (EquipmentItem Item, EquipmentQuote Quote) Quote(string itemId, int quantity)
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
        var order = new EquipmentOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Equipment order {order.Id} by {customerId}");
        return EquipmentMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<EquipmentOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public EquipmentItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
