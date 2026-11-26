namespace RoomBooking.Core.Facilities.Desks;

public interface IDesksRepository
{
    IReadOnlyList<DesksItem> Items();

    DesksItem? Find(string itemId);

    IReadOnlyList<DesksOrder> OrdersFor(string customerId);

    IReadOnlyList<DesksOrder> AllOrders();

    string NextOrderId();

    void Add(DesksOrder order);
}
