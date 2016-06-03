using System;
using System.Collections.Generic;
using System.Linq;

namespace Documents
{
    public interface IPublisher
    {
        void Publish<T>(T message) where T : IMessage;

    }
    public class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, List<Handles>> _subscriptions = new Dictionary<string, List<Handles>>();

        public void Publish<T>(T message) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();

            Publish(topic, message);
            Publish(message.CorrelationId.ToString(), message);
        }

        public void Publish<T>(string topic, T message) where T : IMessage
        {
            if (!_subscriptions.ContainsKey(topic))
            {
                return;
            }
            _subscriptions[topic].ForEach(s =>
            {
                var subscriber = s as Handles<T>;
                subscriber?.Handle(message);
            });

        }


        public void SubscribeByType<T>(Handles<T> subscriber) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            Subscribe(topic, subscriber);
        }

        public void SubscribeByCorrelationId<T>(Handles<T> subscriber, string correlationId) where T : IMessage
        {
            Subscribe(correlationId, subscriber);
        }
        private void Subscribe<T>(string topic, Handles<T> subscriber) where T : IMessage
        {
            lock (this)
            {
                List<Handles> handlers;
                if (!_subscriptions.TryGetValue(topic, out handlers))
                {
                    handlers = new List<Handles>();
                }
                var newList = new List<Handles>(handlers) {subscriber};

                _subscriptions[topic] = newList;
            }
        }
        public void Unsubscribe<T>(Handles<T> subscriber) where T : IMessage
        {
            var topic = typeof(T).Name.ToLower();
            Unsubscribe(topic, subscriber);
        }

        private void Unsubscribe<T>(string topic, Handles<T> subscriber) where T : IMessage
        {
            lock (this)
            {
                List<Handles> handlers;
                if (_subscriptions.TryGetValue(topic, out handlers))
                {
                    var newList = handlers.ToList();
                    newList.Remove(subscriber);
                    _subscriptions[topic] = newList;
                }
            }
        }
    }
}
