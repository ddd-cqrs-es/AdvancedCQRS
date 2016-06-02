using System;
using System.Collections.Generic;
using System.Linq;

namespace Documents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cashier = new Cashier(new Reporter());
            var assMan = new AssistantManager(cashier);

            var cooks = Enumerable.Range(1, 3).Select(i => new Cook(assMan));

            var multiCook = new RoundRobinDispatcher(cooks);

            var waiter = new Waiter(multiCook);

            for (int i = 0; i < 10; i++)
            {
                waiter.PlaceOrder("poo");
            }

            Console.ReadLine();

        }
    }

    public class Reporter : IHandleOrder
    {
        public void Handle(Order order)
        {
            Console.WriteLine(order);
        }
    }
}
