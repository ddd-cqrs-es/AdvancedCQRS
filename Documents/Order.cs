using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Documents
{
    //public class Order2
    //{
    //    public Order2()
    //    {
    //        Id = Guid.NewGuid();
    //        LineItems = new List<LineItem>();
    //    }
    //    public Guid Id { get; private set; }
    //    public int TableName { get; set; }
    //    public List<LineItem> LineItems { get; set; }
    //    public string Ingredients
    //    { get; set; }

    //    public double Tax { get; set; }
    //    public decimal Total { get; set; }
    //    public bool Paid { get; set; }

    //    public void AddLineItem(LineItem item)
    //    {
    //        LineItems.Add(item);
    //    }
    //}


    public class Order
    {
        private readonly JObject _document;

        public Order(string s)
        {
            _document = JObject.Parse(s);
        }

        public Order()
        {
            _document = JObject.Parse("{\"id\" : \"" + Guid.NewGuid()+"\"}");
        }

        public string Id
        {
            get { return _document["id"].Value<string>(); }
        }


        public override string ToString()
        {
            return _document.ToString();
        }

        public int TableNumber
        {
            get { return _document["tableNumber"].Value<int>(); }
        }

        public string Ingredients
        {
            get { return _document["ingredients"].Value<string>(); }
        }

        public List<LineItem> LineItems
        {
            get
            {
                var items = _document["lineItems"];

                return items.Children<JObject>().Select(x => new LineItem
                {
                    Item = x["item"].Value<string>(),
                    Price = x["price"].Value<decimal>(),
                    Quantity = x["quantity"].Value<int>(),
                }).ToList();
            }
        }

        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }


        public Order AddLineItem(LineItem item)
        {
            var items = _document["lineItems"] as JArray;

            items = items ?? new JArray();

            var s = "{\"item\" : \"" +item.Item+ "\", \"quantity\" : " +item.Quantity.ToString() + ", \"price\" : " + item.Price+ "}";
            items.Add(JObject.Parse(s));

            _document["lineItems"] = items;
            return this;
        }

        public Order SetIngredients(string ingredients)
        {
            var prop = _document["ingredients"]  as JValue;

            if (prop == null)
            {
                prop = new JValue(ingredients);

                _document.Add("ingredients", prop);

                return this;
            }

            prop.Value = ingredients;

            _document["ingredients"] = prop;

            return this;
        }
    }

    public class LineItem
    {
        public int Quantity { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }
    }
}
