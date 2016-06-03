using System;
using System.Collections.Generic;

namespace Restaurant.ProcessManagerExample
{
    public class ProcessManagerCoordinator : Handles<OrderPlaced>
    {
        private readonly ProcessManagerFactory _pmf = new ProcessManagerFactory();
        private readonly TopicBasedPubSub _bus;
        private readonly Dictionary<Guid, Handles<IMessage>> _processors = new Dictionary<Guid, Handles<IMessage>>(); 

        public ProcessManagerCoordinator(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            var processManager = _pmf.Create(message.Order, _bus);

            ((BaseProcessManager)processManager).Finish +=  (o, a)=>
            {
                _processors.Remove(message.CorrelationId);
            };

            _bus.SubscribeByCorrelationId(processManager, message.CorrelationId.ToString());
            _processors.Add(message.CorrelationId, processManager);
            
        }

        
    }
}