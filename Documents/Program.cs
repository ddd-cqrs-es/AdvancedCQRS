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
            var bus = new TopicBasedPubSub();

            var reporter = new Reporter();
            bus.SubscribeByType(reporter);

            var cashier = new ThreadedHandler<OrderPriced>(new Cashier(bus), "Cashier");
            startables.Add(cashier);
            bus.SubscribeByType(cashier);

            var assMan = new ThreadedHandler<FoodCooked>(new AssistantManager(bus), "Assistant Manager");
            startables.Add(assMan);
            bus.SubscribeByType(assMan);

            var rnd = new Random(1234);
            var cooks = Enumerable.Range(1, 3).Select(i =>
            {
                var cook = new ThreadedHandler<OrderPlaced>(new Cook(bus, $"cook-{i}", rnd.Next(0, 1000)), "Cook "+i);
                startables.Add(cook);
                return cook;
            });

            var dispatcher = new MoreFairDispatcher<OrderPlaced>(cooks);
            bus.SubscribeByType(dispatcher);
            var waiter = new Waiter(bus);
            return waiter;
        }
    }

    public class Reporter : Handles<OrderPaid>
    {
        public void Handle(OrderPaid message)
        {
            Console.WriteLine(message.Order);
        }
    }
}
