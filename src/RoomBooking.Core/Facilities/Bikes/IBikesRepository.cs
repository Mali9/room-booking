namespace RoomBooking.Core.Facilities.Bikes;

public interface IBikesRepository
{
    IReadOnlyList<BikesItem> Items();

    BikesItem? Find(string itemId);

    IReadOnlyList<BikesOrder> OrdersFor(string customerId);

    IReadOnlyList<BikesOrder> AllOrders();

    string NextOrderId();

    void Add(BikesOrder order);
}
