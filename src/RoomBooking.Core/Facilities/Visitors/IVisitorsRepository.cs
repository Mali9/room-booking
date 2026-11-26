namespace RoomBooking.Core.Facilities.Visitors;

public interface IVisitorsRepository
{
    IReadOnlyList<VisitorsItem> Items();

    VisitorsItem? Find(string itemId);

    IReadOnlyList<VisitorsOrder> OrdersFor(string customerId);

    IReadOnlyList<VisitorsOrder> AllOrders();

    string NextOrderId();

    void Add(VisitorsOrder order);
}
