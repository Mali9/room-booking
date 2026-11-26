namespace RoomBooking.Core.Facilities.Labs;

public interface ILabsRepository
{
    IReadOnlyList<LabsItem> Items();

    LabsItem? Find(string itemId);

    IReadOnlyList<LabsOrder> OrdersFor(string customerId);

    IReadOnlyList<LabsOrder> AllOrders();

    string NextOrderId();

    void Add(LabsOrder order);
}
