namespace RoomBooking.Core.Facilities.Security;

public interface ISecurityRepository
{
    IReadOnlyList<SecurityItem> Items();

    SecurityItem? Find(string itemId);

    IReadOnlyList<SecurityOrder> OrdersFor(string customerId);

    IReadOnlyList<SecurityOrder> AllOrders();

    string NextOrderId();

    void Add(SecurityOrder order);
}
