using System;
using System.Linq;

namespace Documents
{
    public class Waiter
    {
        private readonly IHandleOrder _next;

        public Waiter(IHandleOrder next)
        {
            _next = next;
        }

        public string PlaceOrder(params string[] items)
        {
            var order = new Order();

            items.ToList().Select(LookupItem).ToList().ForEach(x => order.AddLineItem(x));

            Console.WriteLine("Placing order");

            _next.Handle(order);

            return order.Id;
        }

        private LineItem LookupItem(string s)
        {
            return new LineItem()
            {
                Item = s,
                Quantity = 1
            };
        }
    }
}