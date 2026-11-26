namespace RoomBooking.Core.Facilities.Charging;

public interface IChargingRepository
{
    IReadOnlyList<ChargingItem> Items();

    ChargingItem? Find(string itemId);

    IReadOnlyList<ChargingOrder> OrdersFor(string customerId);

    IReadOnlyList<ChargingOrder> AllOrders();

    string NextOrderId();

    void Add(ChargingOrder order);
}
