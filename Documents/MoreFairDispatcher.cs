using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Documents
{
    class MoreFairDispatcher<T> : IHandle<T> where T : IMessage
    {
        private readonly List<ThreadedHandler<T>> _handlers;

        public MoreFairDispatcher(IEnumerable<ThreadedHandler<T>> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(T order)
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