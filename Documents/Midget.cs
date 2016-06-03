using System;

namespace Documents
{
    public class Midget : Handles<IMessage>
    {
        private readonly TopicBasedPubSub _bus;

        public Midget(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(IMessage message)
        {
            if (message is OrderPlaced)
            {
                Console.WriteLine("Midget : OrderPlaced");
                var command = new CookFood()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((OrderPlaced) message).Order,
                };
                _bus.Publish(command);
            }
            if (message is FoodCooked)
            {
                Console.WriteLine("Midget : FoodCooked");
                var command = new PriceOrder()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((FoodCooked) message).Order,
                };
                _bus.Publish(command);
            }
            if (message is OrderPriced)
            {
                Console.WriteLine("Midget : OrderPriced");
                var command = new TakePayment()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((OrderPriced) message).Order,
                };
                _bus.Publish(command);
            }

        }
    }
}