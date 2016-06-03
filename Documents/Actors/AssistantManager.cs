using System;
using System.Linq;

namespace Restaurant.ProcessManagerExample.Actors
{
    public class AssistantManager : Handles<PriceOrder> {
        private readonly IPublisher _bus;

        public AssistantManager(IPublisher bus)
        {
            _bus = bus;
        }

        public void Handle(PriceOrder message)
        {
            var order = message.Order;
            Console.WriteLine("Pricing...");
            
            order.LineItems.ForEach(item => order = order.SetLinePrice(item.Id, 5.10m));
            var subTotal = order.LineItems.Sum(x => x.Price);
            var tax = subTotal * 0.1m;

            order = order.SetTax(tax)
            .SetTotal(tax + subTotal);


            var @event = new OrderPriced { Order = order };
            @event.CorrelationId = message.CorrelationId;
            @event.CausationId = message.Id;

            _bus.Publish(@event);
        }
    }
}