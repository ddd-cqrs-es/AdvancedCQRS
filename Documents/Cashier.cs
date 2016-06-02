using System;

namespace Documents
{
    public class Cashier : IHandleOrder
    {
        private readonly IHandleOrder _next;

        public Cashier(IHandleOrder next)
        {
            _next = next;
        }

        public void Handle(Order order)
        {

            Console.WriteLine("Processing Payment...");

            order = order.SetPaid(true);
            _next.Handle(order);
        }
    }
}