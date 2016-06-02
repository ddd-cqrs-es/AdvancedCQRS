using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Documents
{
    class MoreFairDispatcher : IHandleOrder
    {
        private readonly List<ThreadedHandler> _handlers;

        public MoreFairDispatcher(IEnumerable<ThreadedHandler> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(Order order)
        {
            while (true)
            {
                var chosenHandler = _handlers.FirstOrDefault(h => h.GetCount() < 5);
                if (chosenHandler != null)
                {
                    chosenHandler.Handle(order);
                    break;
                }
                Thread.Sleep(1);
            }

        }

        
    }
}