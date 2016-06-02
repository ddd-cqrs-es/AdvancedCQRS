using System;
using System.Linq;

namespace Documents
{
    public class Waiter
    {
        private readonly IPublisher _bus;

        public Waiter(IPublisher bus)
        {
            _bus = bus;
        }

        public string PlaceOrder(params string[] items)
        {
            var order = new Order();

            items.ToList().Select(LookupItem).ToList().ForEach(x => order = order.AddLineItem(x));

            Console.WriteLine("Placing order");

            _bus.Publish(new OrderPlaced { Order = order});

            return order.Id;
        }

        private LineItem LookupItem(string s)
        {
            return new LineItem()
            {
                Item = s,
                Quantity = 1
            };
        }
    }
}