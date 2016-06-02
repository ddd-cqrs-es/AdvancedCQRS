using System.Collections.Generic;

namespace Documents
{
    class RoundRobinDispatcher : IHandleOrder
    {
        private readonly Queue<IHandleOrder> _handlers;

        public RoundRobinDispatcher(IEnumerable<IHandleOrder> handlers)
        {
            _handlers = new Queue<IHandleOrder>(handlers);
        }

        public void Handle(Order order)
        {
            var handler = _handlers.Peek();
            handler.Handle(order);
            _handlers.Enqueue(_handlers.Dequeue());
        }

        public void AlternativeHandle(Order order)
        {
            var handler = _handlers.Dequeue();
            try
            {
                handler.Handle(order);
            }
            finally
            {
                _handlers.Enqueue(handler);
            }
        }
    }
}