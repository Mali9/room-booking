namespace RoomBooking.Core.Facilities.Shuttle;

public interface IShuttleRepository
{
    IReadOnlyList<ShuttleItem> Items();

    ShuttleItem? Find(string itemId);

    IReadOnlyList<ShuttleOrder> OrdersFor(string customerId);

    IReadOnlyList<ShuttleOrder> AllOrders();

    string NextOrderId();

    void Add(ShuttleOrder order);
}
