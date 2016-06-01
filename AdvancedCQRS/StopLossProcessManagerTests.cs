using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdvancedCQRS
{
    class StopLossProcessManagerTests
    {
        
        [Test]
        public void WhenPositionAcquiredIsPublished_StopLossPriceIsUpdated()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);

            sut.Handle(new PositionAcquired {Price = 105});

            var message = publisher.FindMessage<SendToMeIn>();
            Assert.IsInstanceOf<RemoveFrom10SecondWindow>(message.Inner);
            Assert.That(message.DelayInSeconds == 10);
            var message2 = publisher.FindMessage<StopLossPriceUpdated>();
            Assert.That(message2.Price == 95);
        }

        [Test]
        public void GivenPriceUpdated_ThenRemoveIn10SecondsIsPublished()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired {Price = 100});
            publisher.PublishedMessages.Clear();

            var priceUpdated = new PriceUpdated { Price = 95 };
            sut.Handle(priceUpdated);


            var message = (SendToMeIn)publisher.PublishedMessages.First();
            Assert.IsInstanceOf<RemoveFrom10SecondWindow>(message.Inner);
            Assert.That(message.DelayInSeconds == 10);
        }

        [Test]
        public void GivenPriceUodated_WhenRemovingIn10Seconds_ThenStopLossPriceUpdated()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired { Price = 100 });
            var priceUpdated = new PriceUpdated { Price = 101 };
            sut.Handle(priceUpdated);
            publisher.PublishedMessages.Clear();

            sut.Handle(new RemoveFrom10SecondWindow {Price = 100});

            StopLossPriceUpdated actual = publisher.FindMessage<StopLossPriceUpdated>();
            Assert.AreEqual(91, actual.Price);
        }

        [Test]
        public void GivePositionAcquired_ThenRemoveFrom13SencondWindowIsPublished()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);

            sut.Handle(new PositionAcquired { Price = 100 });

            var message = publisher.PublishedMessages.OfType<SendToMeIn>().SingleOrDefault(x => x.Inner is RemoveFrom13SecondWindow);
            Assert.That(message.DelayInSeconds == 13);
            var innerMessage = (RemoveFrom13SecondWindow)message.Inner;
            Assert.That(innerMessage.Price == 100);
        }

        [Test]
        public void GivenPositionAcquired_WhenPriceUopdated_ThenRemoveFrom13SencondWindowIsPublished()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired { Price = 100 });
            publisher.PublishedMessages.Clear();

            sut.Handle(new PriceUpdated { Price = 90 });

            var message = publisher.PublishedMessages.OfType<SendToMeIn>().SingleOrDefault(x => x.Inner is RemoveFrom13SecondWindow);
            Assert.That(message.DelayInSeconds == 13);
            var innerMessage = (RemoveFrom13SecondWindow) message.Inner;
            Assert.That(innerMessage.Price == 90);
        }

        [Test]
        public void GivenDecreasingPrice_ThenWeTriggerHitStopLost()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired { Price = 100 });
            sut.Handle(new PriceUpdated { Price = 89 });
            publisher.PublishedMessages.Clear();

            sut.Handle(new RemoveFrom13SecondWindow { Price = 100 });

            var message = publisher.PublishedMessages.First();
            Assert.IsInstanceOf<StopLossHit>(message);
        }

        [Test]
        public void GivenDecreasingPrice_ThenWeTriggerHitStopLost_X()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired { Price = 100 });
            sut.Handle(new PriceUpdated { Price = 89 });
            sut.Handle(new PriceUpdated { Price = 89 });
            publisher.PublishedMessages.Clear();

            sut.Handle(new RemoveFrom13SecondWindow { Price = 100 });
            sut.Handle(new RemoveFrom13SecondWindow { Price = 89 });

            var message = publisher.PublishedMessages.Single();
            Assert.IsInstanceOf<StopLossHit>(message);
        }

        [Test]
        public void GivenDecreasingPrice_ThenWeShouldNotReduceTheStopLossPrice()
        {
            var publisher = new FakePublisher();
            var sut = new StopLossProcessManager(publisher);
            sut.Handle(new PositionAcquired { Price = 105 });
            sut.Handle(new PriceUpdated { Price = 95 });
            publisher.PublishedMessages.Clear();

            sut.Handle(new RemoveFrom10SecondWindow { Price = 105 }); 

            var message = publisher.FindMessage<StopLossPriceUpdated>();
            Assert.IsNull(message);
        }
    }
}
