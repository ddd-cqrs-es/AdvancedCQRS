using System;

namespace Restaurant.ProcessManagerExample.Actors
{
    public class Waiter
    {
        private readonly IPublisher _bus;

        public Waiter(IPublisher bus)
        {
            _bus = bus;
        }

        public string PlaceOrder(string items, bool isDodgy)
        {
            var order = new Order();
            order = order.SetDodgyCustomer(isDodgy);
            order = order.AddLineItem(LookupItem(items));

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