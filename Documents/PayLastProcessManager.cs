using System;

namespace Restaurant.ProcessManagerExample
{
    public abstract class BaseProcessManager
    {
        public event EventHandler Finish;

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, EventArgs.Empty);
        }
    }

    public class PayLastProcessManager : BaseProcessManager, Handles<IMessage>
    {
        private bool isCooked;
        private readonly TopicBasedPubSub _bus;

        public PayLastProcessManager(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(IMessage message)
        {
            if (message is OrderPlaced)
            {
                Console.WriteLine("PayLastProcessManager : OrderPlaced");
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
                Console.WriteLine("PayLastProcessManager : FoodCooked");
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
                Console.WriteLine("PayLastProcessManager : OrderPriced");
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
                this.OnFinish();
            }

        }
    }
}