using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Lists, quotes and places Translation orders.</summary>
public sealed class TranslationService(
    ITranslationRepository repository,
    TranslationValidator validator,
    TranslationPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<TranslationItem> ListItems() => repository.Items();

    public (TranslationItem Item, TranslationQuote Quote) Quote(string itemId, int quantity)
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
        var order = new TranslationOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Translation order {order.Id} by {customerId}");
        return TranslationMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<TranslationOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public TranslationItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
