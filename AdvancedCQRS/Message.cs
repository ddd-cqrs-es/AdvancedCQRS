using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedCQRS
{
    public abstract class Message{}

    public class PositionAcquired : Message
    {
        public int Price { get; set; }
    }

    public class PriceUpdated : Message
    {
        public int Price { get; set; }
    }
    public class StopLossHit : Message { }

    public class StopLossPriceUpdated : Message
    {
        public int Price { get; set; }
    }

    public class SendToMeIn : Message
    {
        public Message Inner { get; set; }
        public int DelayInSeconds { get; set; }
    }

    public class RemoveFrom10SecondWindow : Message
    {
        public  int Price { get; set; }
    }
    public class RemoveFrom13SecondWindow : Message
    {
        public  int Price { get; set; }
    }
}
