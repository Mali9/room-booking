using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Photography;

/// <summary>Lists, quotes and places Photography orders.</summary>
public sealed class PhotographyService(
    IPhotographyRepository repository,
    PhotographyValidator validator,
    PhotographyPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<PhotographyItem> ListItems() => repository.Items();

    public (PhotographyItem Item, PhotographyQuote Quote) Quote(string itemId, int quantity)
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
        var order = new PhotographyOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Photography order {order.Id} by {customerId}");
        return PhotographyMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<PhotographyOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public PhotographyItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
