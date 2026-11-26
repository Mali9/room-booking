namespace RoomBooking.Core.Facilities.Equipment;

public interface IEquipmentRepository
{
    IReadOnlyList<EquipmentItem> Items();

    EquipmentItem? Find(string itemId);

    IReadOnlyList<EquipmentOrder> OrdersFor(string customerId);

    IReadOnlyList<EquipmentOrder> AllOrders();

    string NextOrderId();

    void Add(EquipmentOrder order);
}
