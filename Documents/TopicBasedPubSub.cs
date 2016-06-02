using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public interface IPublisher
    {
        void Publish(string topic, Order order);
        
    }
    class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, IHandleOrder> _subscriptions = new Dictionary<string, IHandleOrder>(); 
        public void Publish(string topic, Order order)
        {
            _subscriptions[topic].Handle(order);
        }

        public void Subscribe(string topic, IHandleOrder subscriber)
        {
            _subscriptions.Add(topic, subscriber);
        }
    }
}
