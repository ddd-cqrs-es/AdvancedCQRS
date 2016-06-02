using System;
using System.Linq;

namespace Documents
{
    public class AssistantManager : IHandleOrder {
        private readonly IHandleOrder _next;

        public AssistantManager(IHandleOrder next)
        {
            _next = next;
        }

        public void Handle(Order order)
        {

            Console.WriteLine("Pricing...");

            
            order.LineItems.ForEach(item => order = order.SetLinePrice(item.Id, 5.10m));
            var subTotal = order.LineItems.Sum(x => x.Price);
            var tax = subTotal * 0.1m;

            order = order.SetTax(tax)
            .SetTotal(tax + subTotal);

            _next.Handle(order);
        }
    }
}