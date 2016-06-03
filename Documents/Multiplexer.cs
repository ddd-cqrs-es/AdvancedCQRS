using System.Collections.Generic;
using System.Linq;

namespace Restaurant.ProcessManagerExample
{
    class Multiplexer<T> : Handles<T> where T : IMessage
    {
        private readonly List<Handles<T>> _handlers;

        public Multiplexer(IEnumerable<Handles<T>> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(T message)
        {
            _handlers.ForEach(x => x.Handle(message));
        }
    }
}
