using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Documents
{
    class Multiplexer<T> : IHandle<T> where T : IMessage
    {
        private readonly List<IHandle<T>> _handlers;

        public Multiplexer(IEnumerable<IHandle<T>> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(T order)
        {
            _handlers.ForEach(x => x.Handle(order));
        }
    }
}
