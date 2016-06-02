using System;
using System.Linq;

namespace Documents
{
    public class AssistantManager : IHandleOrder {
        private readonly IPublisher _bus;

        public AssistantManager(IPublisher bus)
        {
            _bus = bus;
        }

        public void Handle(Order order)
        {

            Console.WriteLine("Pricing...");
            
            order.LineItems.ForEach(item => order = order.SetLinePrice(item.Id, 5.10m));
            var subTotal = order.LineItems.Sum(x => x.Price);
            var tax = subTotal * 0.1m;

            order = order.SetTax(tax)
            .SetTotal(tax + subTotal);

            _bus.Publish("pay", order);
        }
    }
}