using System;
using System.Threading;

namespace Documents
{
    public class Cook : IHandleOrder
    {
        private readonly IHandleOrder _next;
        private readonly string _name;
        private readonly int _delayMillisecond;

        public Cook(IHandleOrder next, string name, int delayMillisecond)
        {
            _next = next;
            _name = name;
            _delayMillisecond = delayMillisecond;
        }

        public void Handle(Order order)
        {
            order = order.SetIngredients("bread,meat");

            Console.WriteLine(_name + " is cooking...");
            Thread.Sleep(_delayMillisecond);

            _next.Handle(order);
        }
    }
}