namespace RoomBooking.Core.Facilities.ItSupport;

public interface IItSupportRepository
{
    IReadOnlyList<ItSupportItem> Items();

    ItSupportItem? Find(string itemId);

    IReadOnlyList<ItSupportOrder> OrdersFor(string customerId);

    IReadOnlyList<ItSupportOrder> AllOrders();

    string NextOrderId();

    void Add(ItSupportOrder order);
}
