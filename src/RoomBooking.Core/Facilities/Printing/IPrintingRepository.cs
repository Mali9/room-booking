namespace RoomBooking.Core.Facilities.Printing;

public interface IPrintingRepository
{
    IReadOnlyList<PrintingItem> Items();

    PrintingItem? Find(string itemId);

    IReadOnlyList<PrintingOrder> OrdersFor(string customerId);

    IReadOnlyList<PrintingOrder> AllOrders();

    string NextOrderId();

    void Add(PrintingOrder order);
}
