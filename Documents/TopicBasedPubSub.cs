using System.Collections.Generic;

namespace Documents
{
    public interface IPublisher
    {
        void Publish<T>(T message) where T : IMessage;

    }
    class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, Handles> _subscriptions = new Dictionary<string, Handles>();

        public void Publish<T>(T message) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            ((Handles<T>)_subscriptions[topic]).Handle(message);
        }

        public void SubscribeByType<T>(Handles<T> subscriber) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            _subscriptions.Add(topic, subscriber);
        }
        
    }
}
