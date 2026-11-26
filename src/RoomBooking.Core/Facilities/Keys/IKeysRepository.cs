namespace RoomBooking.Core.Facilities.Keys;

public interface IKeysRepository
{
    IReadOnlyList<KeysItem> Items();

    KeysItem? Find(string itemId);

    IReadOnlyList<KeysOrder> OrdersFor(string customerId);

    IReadOnlyList<KeysOrder> AllOrders();

    string NextOrderId();

    void Add(KeysOrder order);
}
