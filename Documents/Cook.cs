using System;
using System.Threading;

namespace Documents
{
    public class Cook : IHandleOrder
    {
        private readonly IHandleOrder _next;

        public Cook(IHandleOrder next)
        {
            _next = next;
        }

        public void Handle(Order order)
        {
            order = order.SetIngredients("bread,meat");

            Console.WriteLine("Cooking...");
            Thread.Sleep(2000);

            _next.Handle(order);
        }
    }
}