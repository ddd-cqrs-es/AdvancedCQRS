using System.Collections.Generic;

namespace Documents
{
    class RoundRobinDispatcher<T> : Handles<T> where T : IMessage
    {
        private readonly Queue<Handles<T>> _handlers;

        public RoundRobinDispatcher(IEnumerable<Handles<T>> handlers)
        {
            _handlers = new Queue<Handles<T>>(handlers);
        }

        public void Handle(T message)
        {
            var handler = _handlers.Peek();
            handler.Handle(message);
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