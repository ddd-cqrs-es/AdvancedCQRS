using System;
using System.Threading;

namespace Restaurant.ProcessManagerExample.Actors
{
    public class Cook : Handles<CookFood>
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

        public void Handle(CookFood message)
        {
            var order = message.Order.SetIngredients("bread,meat");

            Console.WriteLine(_name + " is cooking...");
            Thread.Sleep(_delayMillisecond);


            var @event = new FoodCooked {Order = order};
            @event.CorrelationId = message.CorrelationId;
            @event.CausationId = message.Id;

            _bus.Publish(@event);
        }
    }
}