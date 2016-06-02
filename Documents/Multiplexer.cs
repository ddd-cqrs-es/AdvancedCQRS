using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Documents
{
    class Multiplexer<T> : Handles<T> where T : IMessage
    {
        private readonly List<Handles<T>> _handlers;

        public Multiplexer(IEnumerable<Handles<T>> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(T order)
        {
            _handlers.ForEach(x => x.Handle(order));
        }
    }
}
