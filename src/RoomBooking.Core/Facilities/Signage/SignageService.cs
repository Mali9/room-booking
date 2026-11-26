using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Signage;

/// <summary>Lists, quotes and places Signage orders.</summary>
public sealed class SignageService(
    ISignageRepository repository,
    SignageValidator validator,
    SignagePricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<SignageItem> ListItems() => repository.Items();

    public (SignageItem Item, SignageQuote Quote) Quote(string itemId, int quantity)
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
        var order = new SignageOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Signage order {order.Id} by {customerId}");
        return SignageMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<SignageOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public SignageItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
