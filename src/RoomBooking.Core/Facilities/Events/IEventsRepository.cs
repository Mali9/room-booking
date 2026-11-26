namespace RoomBooking.Core.Facilities.Events;

public interface IEventsRepository
{
    IReadOnlyList<EventsItem> Items();

    EventsItem? Find(string itemId);

    IReadOnlyList<EventsOrder> OrdersFor(string customerId);

    IReadOnlyList<EventsOrder> AllOrders();

    string NextOrderId();

    void Add(EventsOrder order);
}
