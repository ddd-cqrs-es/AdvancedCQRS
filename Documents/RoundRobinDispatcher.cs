using System.Collections.Generic;

namespace Documents
{
    class RoundRobinDispatcher<T> : IHandle<T> where T : IMessage
    {
        private readonly Queue<IHandle<T>> _handlers;

        public RoundRobinDispatcher(IEnumerable<IHandle<T>> handlers)
        {
            _handlers = new Queue<IHandle<T>>(handlers);
        }

        public void Handle(T order)
        {
            var handler = _handlers.Peek();
            handler.Handle(order);
            _handlers.Enqueue(_handlers.Dequeue());
        }

        public void AlternativeHandle(T order)
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