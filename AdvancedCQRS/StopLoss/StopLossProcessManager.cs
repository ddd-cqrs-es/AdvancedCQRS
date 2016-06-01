using System.Collections.Generic;
using System.Linq;
using AdvancedCQRS.StopLoss;

namespace AdvancedCQRS
{
    public class StopLossProcessManager
    {
        private readonly IPublishMessages _publisher;

        readonly List<int> _price10SecondWindow = new List<int>();
        readonly List<int> _price13SecondWindow = new List<int>();
        private bool _isStopLossHit;
        public int StopLossPrice { get; set; }

        public StopLossProcessManager(IPublishMessages publisher)
        {
            _publisher = publisher;
        }

        public void Handle(PositionAcquired message)
        {
            _price10SecondWindow.Add(message.Price);
            _price13SecondWindow.Add(message.Price);

            UpdateStopLossPrice();
            _publisher.Publish(new SendToMeIn { DelayInSeconds = 10, Inner = new RemoveFrom10SecondWindow { Price = message.Price } });
            _publisher.Publish(new SendToMeIn { DelayInSeconds = 13, Inner = new RemoveFrom13SecondWindow { Price = message.Price } });
        }
        public void Handle(PriceUpdated message)
        {
            _price10SecondWindow.Add(message.Price);
            _price13SecondWindow.Add(message.Price);
            _publisher.Publish(new SendToMeIn { DelayInSeconds = 10, Inner = new RemoveFrom10SecondWindow() { Price = message.Price } });
            _publisher.Publish(new SendToMeIn { DelayInSeconds = 13, Inner = new RemoveFrom13SecondWindow { Price = message.Price } });
        }

        public void Handle(RemoveFrom13SecondWindow message)
        {
            _price13SecondWindow.Remove(message.Price);

            if (_isStopLossHit)
                return;
            var max = _price13SecondWindow.Max();
            if (max < StopLossPrice)
            {
                _isStopLossHit = true;
                _publisher.Publish(new StopLossHit());
            }
        }
        public void Handle(RemoveFrom10SecondWindow message)
        {
            _price10SecondWindow.Remove(message.Price);
            UpdateStopLossPrice();
        }

        private void UpdateStopLossPrice()
        {
            if (!_price10SecondWindow.Any())
            {
                return;
            }
            var newStopLossPrice = _price10SecondWindow.Min() - 10;
            if (newStopLossPrice > StopLossPrice)
            {
                StopLossPrice = newStopLossPrice;
                _publisher.Publish(new StopLossPriceUpdated { Price = newStopLossPrice });
            }
        }
    }
}