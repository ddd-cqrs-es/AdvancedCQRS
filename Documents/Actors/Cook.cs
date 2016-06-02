using System;
using System.Threading;

namespace Documents
{
    public class Cook : Handles<OrderPlaced>
    {
        private readonly IPublisher _bus;
        private readonly string _name;
        private readonly int _delayMillisecond;

        public Cook(IPublisher bus, string name, int delayMillisecond)
        {
            _bus = bus;
            _name = name;
            _delayMillisecond = delayMillisecond;
        }

        public void Handle(OrderPlaced message)
        {
            var order = message.Order.SetIngredients("bread,meat");

            Console.WriteLine(_name + " is cooking...");
            Thread.Sleep(_delayMillisecond);

            _bus.Publish(new FoodCooked {Order = order});
        }
    }
}