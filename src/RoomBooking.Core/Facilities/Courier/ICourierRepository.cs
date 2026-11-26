namespace RoomBooking.Core.Facilities.Courier;

public interface ICourierRepository
{
    IReadOnlyList<CourierItem> Items();

    CourierItem? Find(string itemId);

    IReadOnlyList<CourierOrder> OrdersFor(string customerId);

    IReadOnlyList<CourierOrder> AllOrders();

    string NextOrderId();

    void Add(CourierOrder order);
}
