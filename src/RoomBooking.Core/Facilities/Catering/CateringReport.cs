namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Revenue of Catering, priced with today's rules.</summary>
public sealed class CateringReport(ICateringRepository repository, CateringPricing pricing)
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
