using System;

namespace Documents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cashier = new Cashier(new Reporter());
            var assMan = new AssistantManager(cashier);

            var cook1 = new Cook(assMan);
            var cook2 = new Cook(assMan);
            var multiCook = new Multiplexer(new []{cook1, cook2});

            var waiter = new Waiter(multiCook);

            for (int i = 0; i < 1; i++)
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
