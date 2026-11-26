using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>Lists, quotes and places Maintenance orders.</summary>
public sealed class MaintenanceService(
    IMaintenanceRepository repository,
    MaintenanceValidator validator,
    MaintenancePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<MaintenanceItem> ListItems() => repository.Items();

    public (MaintenanceItem Item, MaintenanceQuote Quote) Quote(string itemId, int quantity)
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
        var order = new MaintenanceOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Maintenance order {order.Id} by {customerId}");
        return MaintenanceMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<MaintenanceOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public MaintenanceItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
