namespace RoomBooking.Core.Facilities.Library;

public interface ILibraryRepository
{
    IReadOnlyList<LibraryItem> Items();

    LibraryItem? Find(string itemId);

    IReadOnlyList<LibraryOrder> OrdersFor(string customerId);

    IReadOnlyList<LibraryOrder> AllOrders();

    string NextOrderId();

    void Add(LibraryOrder order);
}
