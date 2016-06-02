using System;

namespace Documents
{
    public class Cashier : IHandleOrder
    {
        private readonly IPublisher _publisher;

        public Cashier(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(Order order)
        {
            Console.WriteLine("Processing Payment...");

            order = order.SetPaid(true);
            _publisher.Publish("done", order);
        }
    }
}