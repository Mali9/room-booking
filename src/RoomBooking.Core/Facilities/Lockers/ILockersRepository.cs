namespace RoomBooking.Core.Facilities.Lockers;

public interface ILockersRepository
{
    IReadOnlyList<LockersItem> Items();

    LockersItem? Find(string itemId);

    IReadOnlyList<LockersOrder> OrdersFor(string customerId);

    IReadOnlyList<LockersOrder> AllOrders();

    string NextOrderId();

    void Add(LockersOrder order);
}
