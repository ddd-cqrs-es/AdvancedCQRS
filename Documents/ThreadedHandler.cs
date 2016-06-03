using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.ProcessManagerExample
{
    class ThreadedHandler<T> : Handles<T>, IStartable, IMonitorQueue where T: IMessage
    {
        private readonly Handles<T> _handler;
        private readonly string _name;
        readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private Task _task;


        public ThreadedHandler(Handles<T> handler, string name)
        {
            _handler = handler;
            _name = name;
        }

        public void Handle(T message)
        {
            _queue.Enqueue(message);
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
                T order;
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