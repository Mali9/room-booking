using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>Lists, quotes and places Uniforms orders.</summary>
public sealed class UniformsService(
    IUniformsRepository repository,
    UniformsValidator validator,
    UniformsPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<UniformsItem> ListItems() => repository.Items();

    public (UniformsItem Item, UniformsQuote Quote) Quote(string itemId, int quantity)
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
        var order = new UniformsOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Uniforms order {order.Id} by {customerId}");
        return UniformsMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<UniformsOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public UniformsItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
