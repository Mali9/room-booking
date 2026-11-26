namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>Revenue of Laundry, priced with today's rules.</summary>
public sealed class LaundryReport(ILaundryRepository repository, LaundryPricing pricing)
{
    public (int Orders, decimal Revenue) Summarize()
    {
        var orders = repository.AllOrders();
        var revenue = 0m;
        foreach (var order in orders)
        {
            var item = repository.Find(order.ItemId);
            if (item is not null)
            {
                revenue += pricing.Quote(item, order.Quantity).Total;
            }
        }

        return (orders.Count, revenue);
    }
}
