using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace Documents
{
    [TestFixture]
    public class TestFixture
    {
        [Test]
        public void CanReadTableNumber()
        {
            var orderJson = "{\"tableNumber\" : 23}";

            var order = new Order(orderJson);
            order.TableNumber.ShouldBe(23);

        }

        [Test]
        public void CanReadLineItems()
        {
            var orderJson = "{\"lineItems\" : [{ \"quantity\" : 2, \"price\" : 2.00, \"item\" : \"razor blade icecream\"}]}";

            var order = new Order(orderJson);
            order.LineItems[0].Item.ShouldBe("razor blade icecream");

        }

        [Test]
        public void CanAddLineItem()
        {
            var orderJson = "{\"lineItems\" : [{ \"quantity\" : 2, \"price\" : 2.00, \"item\" : \"razor blade icecream\"}]}";

            var order = new Order(orderJson);

            order.AddLineItem(new LineItem {Item = "poo", Price = 3.45m, Quantity = 3});
            order.LineItems[1].Item.ShouldBe("poo");

        }

        [Test]
        public void EnsureExtraProperptyRemainsIntact()
        {
            var orderJson = "{\"tableNumber\" : 23, \"Foo\" : \"Bar\"}";

            var order = new Order(orderJson);
            order.TableNumber.ShouldBe(23);

            var actual = JObject.Parse(order.ToString());
            actual["Foo"].Value<string>().ShouldBe("Bar");

        }
    }
}
