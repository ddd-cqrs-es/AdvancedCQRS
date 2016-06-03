using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Restaurant.ProcessManagerExample.Actors;

namespace Restaurant.ProcessManagerExample
{
    public class OrderMonitor : Handles<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Monitor" + message.Id);
            Console.ResetColor();
        }
    }


    public class Program 
    {
        public static void Main(string[] args)
        {
            List<IStartable> startables = new List<IStartable>();
            var bus = new TopicBasedPubSub();

            var monitor = new OrderMonitor();

            var waiter = SetUp(startables, bus);

            var coordinator = new ProcessManagerCoordinator(bus);

            StartMonitoring(startables);
            bus.SubscribeByType(coordinator);

            startables.ForEach(x => x.Start());

            for (int i = 0; i < 10; i++)
            {
                var orderId = waiter.PlaceOrder("poo", i % 2 == 0);
                bus.SubscribeByCorrelationId(monitor, orderId);

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

        private static Waiter SetUp(List<IStartable> startables, TopicBasedPubSub bus)
        {

            var reporter = new Reporter();
            bus.SubscribeByType<OrderPaid>(reporter);
            bus.SubscribeByType<OrderPlaced>(reporter);
            

            var cashier = new ThreadedHandler<TakePayment>(new Cashier(bus), "Cashier");
            startables.Add(cashier);
            bus.SubscribeByType(cashier);

            var assMan = new ThreadedHandler<PriceOrder>(new AssistantManager(bus), "Assistant Manager");
            startables.Add(assMan);
            bus.SubscribeByType(assMan);

            var rnd = new Random(1234);
            var cooks = Enumerable.Range(1, 3).Select(i =>
            {
                var cook = new ThreadedHandler<CookFood>(new Cook(bus, $"cook-{i}", rnd.Next(0, 1000)), "Cook "+i);
                startables.Add(cook);
                return cook;
            });

            var dispatcher = new MoreFairDispatcher<CookFood>(cooks);
            bus.SubscribeByType(dispatcher);
            var waiter = new Waiter(bus);
            return waiter;
        }

    }




}
