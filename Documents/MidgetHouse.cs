using System;
using System.Collections.Generic;

namespace Documents
{
    public class MidgetHouse<T> : Handles<T> where T : IMessage 
    {
        private readonly TopicBasedPubSub _bus;
        private Dictionary<Guid, Midget> _midgets = new Dictionary<Guid, Midget>(); 

        public MidgetHouse(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(T message)  {
            var midget = new Midget(_bus);

            _bus.SubscribeByCorrelationId<IMessage>(midget, message.CorrelationId.ToString());
            _midgets.Add(message.CorrelationId, midget);
            
        }


    }
}