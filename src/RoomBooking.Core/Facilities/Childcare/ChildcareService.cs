using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Lists, quotes and places Childcare orders.</summary>
public sealed class ChildcareService(
    IChildcareRepository repository,
    ChildcareValidator validator,
    ChildcarePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<ChildcareItem> ListItems() => repository.Items();

    public (ChildcareItem Item, ChildcareQuote Quote) Quote(string itemId, int quantity)
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
        var order = new ChildcareOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Childcare order {order.Id} by {customerId}");
        return ChildcareMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<ChildcareOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public ChildcareItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
