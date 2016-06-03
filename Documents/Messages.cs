using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public interface IMessage
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
        Guid CausationId { get; }
    }
    public abstract class MessageBase : IMessage
    {
        public MessageBase()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
    }

    public class OrderPlaced : MessageBase
    {
        public Order Order { get; set; }
    }
    public class FoodCooked : MessageBase
    {
        public Order Order { get; set; }
    }
    public class OrderPriced : MessageBase
    {
        public Order Order { get; set; }
    }
    public class OrderPaid : MessageBase
    {
        public Order Order { get; set; }
    }
    public class CookFood : MessageBase
    {
        public Order Order { get; set; }
    }
    public class PriceOrder : MessageBase
    {
        public Order Order { get; set; }
    }

    public class TakePayment: MessageBase
    {
        public Order Order { get; set; }
    }


}
