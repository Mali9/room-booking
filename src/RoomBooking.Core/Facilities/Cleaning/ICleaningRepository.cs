namespace RoomBooking.Core.Facilities.Cleaning;

public interface ICleaningRepository
{
    IReadOnlyList<CleaningItem> Items();

    CleaningItem? Find(string itemId);

    IReadOnlyList<CleaningOrder> OrdersFor(string customerId);

    IReadOnlyList<CleaningOrder> AllOrders();

    string NextOrderId();

    void Add(CleaningOrder order);
}
