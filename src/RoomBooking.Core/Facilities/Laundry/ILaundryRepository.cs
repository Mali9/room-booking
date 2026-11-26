namespace RoomBooking.Core.Facilities.Laundry;

public interface ILaundryRepository
{
    IReadOnlyList<LaundryItem> Items();

    LaundryItem? Find(string itemId);

    IReadOnlyList<LaundryOrder> OrdersFor(string customerId);

    IReadOnlyList<LaundryOrder> AllOrders();

    string NextOrderId();

    void Add(LaundryOrder order);
}
