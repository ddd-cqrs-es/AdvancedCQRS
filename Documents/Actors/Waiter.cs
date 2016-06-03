using System;
using System.Linq;

namespace Documents.Actors
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

            var message = new OrderPlaced {Order = order};
            message.CorrelationId= Guid.Parse(order.Id);
            message.CausationId=Guid.Empty;

            _bus.Publish(message);


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