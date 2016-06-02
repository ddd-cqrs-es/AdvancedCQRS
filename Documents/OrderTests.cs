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
    public class OrderTests
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

            var newOrder = order.AddLineItem(new LineItem {Item = "poo", Price = 3.45m, Quantity = 3});

            order.LineItems.Count.ShouldBe(1);

            newOrder.LineItems.Count.ShouldBe(2);

            newOrder.LineItems[1].Item.ShouldBe("poo");

            Object.ReferenceEquals(order, newOrder).ShouldBe(false);

        }

        [Test]
        public void CanAddLineItemWhenNotAlreadyPresent()
        {
            var json = "{}";

            var order = new Order(json);

            var newOrder = order.AddLineItem(new LineItem {Item = "poo", Price = 3.45m, Quantity = 3});

            order.LineItems.Count.ShouldBe(0);

            newOrder.LineItems.Count.ShouldBe(1);
            newOrder.LineItems[0].Item.ShouldBe("poo");

            Object.ReferenceEquals(order, newOrder).ShouldBe(false);

        }

        [Test]
        public void CanSetPri()
        {
            var item = new LineItem {Item = "poo", Price = 3.45m, Quantity = 3};
            var order = new Order().AddLineItem(item);
            order.LineItems.Count.ShouldBe(1);

            var newOrder = order.SetLinePrice(item.Id, 5.00m);

            var newItem = newOrder.LineItems.Single(x => x.Id == item.Id);
            newItem.Price.ShouldBe(5.00m);

        }

        [Test]
        public void CanSetIngredientsWhenNotAlreadyPresent()
        {
            var orderJson = "{\"lineItems\" : [{ \"quantity\" : 2, \"price\" : 2.00, \"item\" : \"razor blade icecream\"}]}";

            var order = new Order(orderJson);
            order.Ingredients.ShouldBe(null);

            var ingredients = "bread, meat";

            var newOrder = order.SetIngredients(ingredients);
            order.Ingredients.ShouldBe(null);

            newOrder.Ingredients.ShouldBe(ingredients);
            Object.ReferenceEquals(order, newOrder).ShouldBe(false);

        }

        [Test]
        public void CanSetIngredientsWhenAlreadyPresent()
        {
            var orderJson = @"

    {
        lineItems : [{ 
            quantity : 2, 
            price : 2.00, 
            item : ""razor blade icecream""
        }],
        ingredients : ""pigs ears""
    }

";

            var order = new Order(orderJson);
            order.Ingredients.ShouldBe("pigs ears");

            var ingredients = "bread, meat";

            var newOrder = order.SetIngredients(ingredients);
            newOrder.Ingredients.ShouldBe(ingredients);

            Object.ReferenceEquals(order, newOrder).ShouldBe(false);
            order.Ingredients.ShouldBe("pigs ears");


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
