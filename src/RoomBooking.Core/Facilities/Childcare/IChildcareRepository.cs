namespace RoomBooking.Core.Facilities.Childcare;

public interface IChildcareRepository
{
    IReadOnlyList<ChildcareItem> Items();

    ChildcareItem? Find(string itemId);

    IReadOnlyList<ChildcareOrder> OrdersFor(string customerId);

    IReadOnlyList<ChildcareOrder> AllOrders();

    string NextOrderId();

    void Add(ChildcareOrder order);
}
