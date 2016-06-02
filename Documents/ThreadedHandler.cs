using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Documents
{
    class ThreadedHandler : IHandleOrder, IStartable
    {
        private readonly IHandleOrder _handler;
        readonly ConcurrentQueue<Order> _queue = new ConcurrentQueue<Order>();
        

        public ThreadedHandler(IHandleOrder handler)
        {
            _handler = handler;
            
        }

        public void Handle(Order order)
        {
            _queue.Enqueue(order);
        }

        public void Start()
        {
            new Task(Process,TaskCreationOptions.LongRunning).Start();
        }

        private void Process()
        {
            while (true)
            {
                Order order;
                if (_queue.TryDequeue(out order))
                {
                    _handler.Handle(order);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}