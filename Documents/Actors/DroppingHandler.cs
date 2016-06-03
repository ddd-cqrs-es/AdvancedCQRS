using System;

namespace Restaurant.ProcessManagerExample.Actors
{
    public class DroppingHandler<T> : Handles<T> where T : IMessage
    {
        private readonly Handles<T> _inner;
        private Random _rnd;

        public DroppingHandler(Handles<T> inner)
        {
            _rnd = new Random(1234);
            _inner = inner;
        }

        public void Handle(T message)
        {
            var randomNumber = _rnd.Next(0, 100);
            if (randomNumber <= 10)
            {
                //Oops
                return;
            }

            _inner.Handle(message);
        }
    }
}