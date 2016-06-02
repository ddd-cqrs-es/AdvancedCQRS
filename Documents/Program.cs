using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Compatibility;

namespace Documents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<IStartable> startables = new List<IStartable>();

            var waiter = SetUp(startables);

            startables.ForEach(x => x.Start());

            for (int i = 0; i < 10; i++)
            {
                waiter.PlaceOrder("poo");
            }
            
            Console.ReadLine();
        }

        private static Waiter SetUp(List<IStartable> startables)
        {
            var cashier = new ThreadedHandler(new Cashier(new Reporter()));
            startables.Add(cashier);
            var assMan = new ThreadedHandler(new AssistantManager(cashier));
            startables.Add(assMan);

            var cooks = Enumerable.Range(1, 3).Select(i =>
            {
                var cook = new ThreadedHandler(new Cook(assMan));
                startables.Add(cook);
                return cook;
            });

            var multiCook = new RoundRobinDispatcher(cooks);

            var waiter = new Waiter(multiCook);
            return waiter;
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
