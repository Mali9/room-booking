using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Training;

/// <summary>Lists, quotes and places Training orders.</summary>
public sealed class TrainingService(
    ITrainingRepository repository,
    TrainingValidator validator,
    TrainingPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<TrainingItem> ListItems() => repository.Items();

    public (TrainingItem Item, TrainingQuote Quote) Quote(string itemId, int quantity)
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
        var order = new TrainingOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Training order {order.Id} by {customerId}");
        return TrainingMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<TrainingOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public TrainingItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
