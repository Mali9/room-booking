namespace RoomBooking.Core.Facilities.Archive;

public interface IArchiveRepository
{
    IReadOnlyList<ArchiveItem> Items();

    ArchiveItem? Find(string itemId);

    IReadOnlyList<ArchiveOrder> OrdersFor(string customerId);

    IReadOnlyList<ArchiveOrder> AllOrders();

    string NextOrderId();

    void Add(ArchiveOrder order);
}
