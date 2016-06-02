using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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

            items.ToList().Select(LookupItem).ToList().ForEach(x => order = order.AddLineItem(x));

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