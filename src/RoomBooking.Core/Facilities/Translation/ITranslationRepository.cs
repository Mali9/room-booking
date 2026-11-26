namespace RoomBooking.Core.Facilities.Translation;

public interface ITranslationRepository
{
    IReadOnlyList<TranslationItem> Items();

    TranslationItem? Find(string itemId);

    IReadOnlyList<TranslationOrder> OrdersFor(string customerId);

    IReadOnlyList<TranslationOrder> AllOrders();

    string NextOrderId();

    void Add(TranslationOrder order);
}
