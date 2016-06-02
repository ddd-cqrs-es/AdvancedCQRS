using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedCQRS
{
    public class Order
    {
        public Order AddLineNumber()
        {
            var newOrder = this.Clone();
            //make change

            return newOrder;
        }
    }
}
