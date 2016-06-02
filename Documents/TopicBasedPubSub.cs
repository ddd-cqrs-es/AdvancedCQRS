using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public interface IPublisher
    {
        void Publish<T>(T message) where T : IMessage;

    }
    class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, IHandle> _subscriptions = new Dictionary<string, IHandle>();

        public void Publish<T>(T message) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            ((IHandle<T>)_subscriptions[topic]).Handle(message);
        }
        

        public void SubscribeByType<T>(IHandle<T> subscriber) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            _subscriptions.Add(topic, subscriber);
        }
        
    }
}
