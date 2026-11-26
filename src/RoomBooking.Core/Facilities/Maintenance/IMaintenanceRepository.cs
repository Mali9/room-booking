namespace RoomBooking.Core.Facilities.Maintenance;

public interface IMaintenanceRepository
{
    IReadOnlyList<MaintenanceItem> Items();

    MaintenanceItem? Find(string itemId);

    IReadOnlyList<MaintenanceOrder> OrdersFor(string customerId);

    IReadOnlyList<MaintenanceOrder> AllOrders();

    string NextOrderId();

    void Add(MaintenanceOrder order);
}
