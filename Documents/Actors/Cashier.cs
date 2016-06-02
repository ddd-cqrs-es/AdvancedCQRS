using System;

namespace Documents
{
    public class Cashier : Handles<OrderPriced>
    {
        private readonly IPublisher _publisher;

        public Cashier(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(OrderPriced message)
        {
            var order = message.Order;
            Console.WriteLine("Processing Payment...");

            order = order.SetPaid(true);

            _publisher.Publish(new OrderPaid {Order = order});
        }
    }
}