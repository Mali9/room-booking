namespace RoomBooking.Core.Facilities.Badges;

public interface IBadgesRepository
{
    IReadOnlyList<BadgesItem> Items();

    BadgesItem? Find(string itemId);

    IReadOnlyList<BadgesOrder> OrdersFor(string customerId);

    IReadOnlyList<BadgesOrder> AllOrders();

    string NextOrderId();

    void Add(BadgesOrder order);
}
