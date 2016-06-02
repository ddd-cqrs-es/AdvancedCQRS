using System;
using System.Linq;

namespace Documents
{
    public class AssistantManager : Handles<FoodCooked> {
        private readonly IPublisher _bus;

        public AssistantManager(IPublisher bus)
        {
            _bus = bus;
        }

        public void Handle(FoodCooked message)
        {
            var order = message.Order;
            Console.WriteLine("Pricing...");
            
            order.LineItems.ForEach(item => order = order.SetLinePrice(item.Id, 5.10m));
            var subTotal = order.LineItems.Sum(x => x.Price);
            var tax = subTotal * 0.1m;

            order = order.SetTax(tax)
            .SetTotal(tax + subTotal);

            _bus.Publish(new OrderPriced {Order = order});
        }
    }
}