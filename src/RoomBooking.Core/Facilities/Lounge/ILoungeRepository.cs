namespace RoomBooking.Core.Facilities.Lounge;

public interface ILoungeRepository
{
    IReadOnlyList<LoungeItem> Items();

    LoungeItem? Find(string itemId);

    IReadOnlyList<LoungeOrder> OrdersFor(string customerId);

    IReadOnlyList<LoungeOrder> AllOrders();

    string NextOrderId();

    void Add(LoungeOrder order);
}
