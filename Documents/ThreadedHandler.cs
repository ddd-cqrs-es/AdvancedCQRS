using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Documents
{
    class ThreadedHandler : IHandleOrder, IStartable, IMonitorQueue
    {
        private readonly IHandleOrder _handler;
        private readonly string _name;
        readonly ConcurrentQueue<Order> _queue = new ConcurrentQueue<Order>();
        private Task _task;


        public ThreadedHandler(IHandleOrder handler, string name)
        {
            _handler = handler;
            _name = name;
        }

        public void Handle(Order order)
        {
            _queue.Enqueue(order);
        }

        public int GetCount()
        {
            return _queue.Count;
        }

        public string GetName()
        {
            return _name;
        }

        public Task GetTask()
        {
            return _task;
        }

        public void Start()
        {
            _task = new Task(Process, TaskCreationOptions.LongRunning);
            _task.Start();
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