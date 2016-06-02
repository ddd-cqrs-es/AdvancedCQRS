using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Documents
{
    class Multiplexer : IHandleOrder
    {
        private readonly List<IHandleOrder> _handlers;

        public Multiplexer(IEnumerable<IHandleOrder> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Handle(Order order)
        {
            _handlers.ForEach(x => x.Handle(order));
        }
    }
}
