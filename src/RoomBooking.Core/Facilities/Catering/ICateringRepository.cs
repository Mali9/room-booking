namespace RoomBooking.Core.Facilities.Catering;

public interface ICateringRepository
{
    IReadOnlyList<CateringItem> Items();

    CateringItem? Find(string itemId);

    IReadOnlyList<CateringOrder> OrdersFor(string customerId);

    IReadOnlyList<CateringOrder> AllOrders();

    string NextOrderId();

    void Add(CateringOrder order);
}
