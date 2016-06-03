using System;

namespace Restaurant.ProcessManagerExample
{
    public class PayFirstProcessManager : BaseProcessManager, Handles<IMessage>
    {
        private bool isCooked;
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

                var sendToMeMessage = new SendToMeIn()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Delay = DateTime.UtcNow.AddSeconds(5),
                    Message = new RetryCooking()
                    {
                        CorrelationId = message.CorrelationId,
                        CausationId = message.Id,
                        Order = ((OrderPaid)message).Order 
                    }
                };
                _bus.Publish(sendToMeMessage);
            }
            if (message is RetryCooking)
            {
                if (isCooked)
                    return;
                Console.WriteLine("PayFirstProcessManager : Retry");
                var command = new CookFood()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Order = ((RetryCooking)message).Order,
                };
                _bus.Publish(command);

                var sendToMeMessage = new SendToMeIn()
                {
                    CorrelationId = message.CorrelationId,
                    CausationId = message.Id,
                    Delay = DateTime.UtcNow.AddSeconds(5),
                    Message = new RetryCooking()
                    {
                        CorrelationId = message.CorrelationId,
                        CausationId = message.Id,
                        Order = ((RetryCooking)message).Order 
                    }
                };
                _bus.Publish(sendToMeMessage);
            }
            if (message is FoodCooked)
            {
                isCooked = true;
                OnFinish();
            }

        }
    }
}