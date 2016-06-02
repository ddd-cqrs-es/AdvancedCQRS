using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework.Compatibility;

namespace Documents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<IStartable> startables = new List<IStartable>();

            var waiter = SetUp(startables);

            StartMonitoring(startables);


            startables.ForEach(x => x.Start());

            for (int i = 0; i < 100; i++)
            {
                waiter.PlaceOrder("poo");
            }
            
            Console.ReadLine();
        }

        private static void StartMonitoring(List<IStartable> startables)
        {
            var monitorableQueues = startables.OfType<IMonitorQueue>().ToList();
            Timer timer =
                new Timer(
                    _ =>
                    {
                        monitorableQueues.ForEach(x =>
                        {
                            if (x.GetCount() > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Queue size of '{x.GetName()}' is {x.GetCount()} ");
                                Console.ResetColor();
                            }
                        });
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private static Waiter SetUp(List<IStartable> startables)
        {
            var cashier = new ThreadedHandler(new Cashier(new Reporter()), "Cashier");
            startables.Add(cashier);
            var assMan = new ThreadedHandler(new AssistantManager(cashier), "Assistant Manager");
            startables.Add(assMan);

            var rnd = new Random(1234);
            var cooks = Enumerable.Range(1, 3).Select(i =>
            {
                var cook = new ThreadedHandler(new Cook(assMan, $"cook-{i}", rnd.Next(0, 1000)), "Cook "+i);
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
