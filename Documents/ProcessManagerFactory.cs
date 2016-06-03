namespace Restaurant.ProcessManagerExample
{
    public class ProcessManagerFactory
    {
        public Handles<IMessage> Create(Order order, TopicBasedPubSub bus)
        {
            if (order.IsDodgy)
                return new PayFirstProcessManager(bus);
            else
                return new PayLastProcessManager(bus);
        }
    }
}