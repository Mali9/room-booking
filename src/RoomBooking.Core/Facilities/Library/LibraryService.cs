using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Library;

/// <summary>Lists, quotes and places Library orders.</summary>
public sealed class LibraryService(
    ILibraryRepository repository,
    LibraryValidator validator,
    LibraryPricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<LibraryItem> ListItems() => repository.Items();

    public (LibraryItem Item, LibraryQuote Quote) Quote(string itemId, int quantity)
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
        var order = new LibraryOrder
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"Library order {order.Id} by {customerId}");
        return LibraryMessages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<LibraryOrder> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public LibraryItem Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
