namespace RoomBooking.Core.Facilities.Storage;

public interface IStorageRepository
{
    IReadOnlyList<StorageItem> Items();

    StorageItem? Find(string itemId);

    IReadOnlyList<StorageOrder> OrdersFor(string customerId);

    IReadOnlyList<StorageOrder> AllOrders();

    string NextOrderId();

    void Add(StorageOrder order);
}
