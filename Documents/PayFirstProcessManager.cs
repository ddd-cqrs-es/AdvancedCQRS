using System;

namespace Restaurant.ProcessManagerExample
{
    public class ProcessManagerFactory
    {
        public Handles<IMessage> Create(Order order, TopicBasedPubSub bus)
        {
            if (order.IsDodgy)
                return new PayFirstProcessManager(bus);
            else
                return new PayLastProcessManager(bus);
        }
    }
    public class PayFirstProcessManager : BaseProcessManager, Handles<IMessage>
    {
        private readonly TopicBasedPubSub _bus;

        public PayFirstProcessManager(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(IMessage message)
        {
            if (message is OrderPlaced)
            {
                Console.WriteLine("PayFirstProcessManager : OrderPlaced");
                var command = new PriceOrder()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((OrderPlaced) message).Order,
                };
                _bus.Publish(command);
            }
            if (message is OrderPriced)
            {
                Console.WriteLine("PayFirstProcessManager : OrderPriced");
                var command = new TakePayment()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((OrderPriced) message).Order,
                };
                _bus.Publish(command);
            }
            if (message is OrderPaid)
            {
                Console.WriteLine("PayFirstProcessManager : OrderPaid");
                var command = new CookFood()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((OrderPaid)message).Order,
                };
                _bus.Publish(command);
            }
            if (message is FoodCooked)
            {
                OnFinish();
            }

        }
    }
}