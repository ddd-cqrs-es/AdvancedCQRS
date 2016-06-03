using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.ProcessManagerExample.Actors
{
    public class AlarmClock : Handles<SendToMeIn>, IStartable
    {
        private readonly TopicBasedPubSub _bus;
        private List<SendToMeIn> _futureMessages = new List<SendToMeIn>();
        private Timer _timer;

        public AlarmClock(TopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(SendToMeIn message)
        {
            _futureMessages.Add(message);
        }

        public void Start()
        {
            _timer = new Timer(_ =>
            {
                var toSend = _futureMessages.Where(x => x.Delay > DateTime.UtcNow).ToList();
                toSend.ForEach(x =>
                {
                    Console.WriteLine("Resending message id " + x.Id);
                    _bus.Publish(x.Message);
                    _futureMessages.Remove(x);
                });
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
        }
    }
}
