using System;

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

            //price items
            foreach (var item in order.LineItems)
            {
                item.Price = CalcPrice(item);
            }

            //tax
            order.Tax = 25.00m;

            //total
            order.Total = 50.00m;

            _next.Handle(order);
        }

        private decimal CalcPrice(LineItem item)
        {
            return 10.00m;
        }

    
    }
}