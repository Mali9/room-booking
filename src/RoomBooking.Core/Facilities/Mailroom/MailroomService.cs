using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Lists, quotes and places Mailroom orders.</summary>
public sealed class MailroomService(
    IMailroomRepository repository,
    MailroomValidator validator,
    MailroomPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<MailroomItem> ListItems() => repository.Items();

    public (MailroomItem Item, MailroomQuote Quote) Quote(string itemId, int quantity)
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
        var order = new MailroomOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Mailroom order {order.Id} by {customerId}");
        return MailroomMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<MailroomOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public MailroomItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
