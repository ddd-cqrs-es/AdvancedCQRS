using System;

namespace Documents.Actors
{
    public class Reporter : Handles<OrderPaid>, Handles<OrderPlaced>
    {
        public void Handle(OrderPaid message)
        {
            Console.WriteLine(message.Order);
        }

        public void Handle(OrderPlaced message)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("Reporter has observed the OrderPlaced event for order " + message.Order.Id);
            Console.ResetColor();
            
        }
    }
}