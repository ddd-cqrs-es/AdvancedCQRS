using System;

namespace Documents.Actors
{
    public class Cashier : Handles<TakePayment>
    {
        private readonly IPublisher _publisher;

        public Cashier(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(TakePayment message)
        {
            var order = message.Order;
            Console.WriteLine("Processing Payment...");

            order = order.SetPaid(true);

            var @event = new OrderPaid { Order = order };
            @event.CorrelationId = message.CorrelationId;
            @event.CausationId = message.Id;

            _publisher.Publish(@event);
        }
    }
}