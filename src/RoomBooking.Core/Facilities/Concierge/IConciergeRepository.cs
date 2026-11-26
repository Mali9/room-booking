namespace RoomBooking.Core.Facilities.Concierge;

public interface IConciergeRepository
{
    IReadOnlyList<ConciergeItem> Items();

    ConciergeItem? Find(string itemId);

    IReadOnlyList<ConciergeOrder> OrdersFor(string customerId);

    IReadOnlyList<ConciergeOrder> AllOrders();

    string NextOrderId();

    void Add(ConciergeOrder order);
}
