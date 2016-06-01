using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cashier = new Cashier(new Reporter());
            var assMan = new AssistantManager(cashier);
            var cook = new Cook(assMan);
            var waiter = new Waiter(cook);

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
            Console.WriteLine(order.Id);
        }
    }
}
