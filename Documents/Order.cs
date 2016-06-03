using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Documents
{
    public class Order 
    {
        protected readonly JObject Document;

        public Order(string s)
        {
            Document = JObject.Parse(s);
        }

        public Order()
        {
            Document = JObject.Parse("{\"id\" : \"" + Guid.NewGuid()+"\"}");
        }

        protected Order(JObject o)
        {
            Document = o;
        }

        public override string ToString()
        {
            return Document.ToString();
        }

        private T ValueOrDefault<T>(string name)
        {
            var value = Document[name];
            return value == null ? default(T) : value.Value<T>();
        }

        public string Id
        {
            get { return ValueOrDefault<string>("id"); }
        }

        public int TableNumber
        {
            get { return ValueOrDefault<int>("tableNumber"); }
        }

        public string Ingredients
        {
            get { return ValueOrDefault<string>("ingredients"); } 
        }

        public bool IsDodgy
        {
            get { return ValueOrDefault<bool>("isDodgy"); }
        }

        public Order SetIngredients(string value)
        {
            return SetOrAddProperty("ingredients", value);
        }
        public Order SetDodgyCustomer(bool isDodgy)
        {
            return SetOrAddProperty("isDodgy", isDodgy);
        }

        public List<LineItem> LineItems
        {
            get
            {
                var items = Document["lineItems"];

                if (items == null)
                {
                    return new List<LineItem>();
                }
                    
                return items.Children<JObject>().Select(x => new LineItem
                {
                    Id = x["id"]?.Value<string>(),
                    Item = x["item"]?.Value<string>(),
                    Price = x["price"].Value<decimal>(),
                    Quantity = x["quantity"].Value<int>(),
                }).ToList();
            }
        }

        public decimal Tax
        {
            get { return ValueOrDefault<decimal>("tax"); }

        }

        public Order SetTax(decimal value)
        {
            return SetOrAddProperty("tax", value);
        }

        public decimal Total
        {
            get { return ValueOrDefault<decimal>("total"); }
        }

        public Order SetTotal(decimal value)
        {
            return SetOrAddProperty("total", value);
        }

        public bool Paid
        {
            get { return ValueOrDefault<bool>("paid"); }
        }

        public Order SetPaid(bool value)
        {
            return SetOrAddProperty("paid", value);
        }

        public Order AddLineItem(LineItem item)
        {
            var doc = (JObject)Document.DeepClone();

            var items = doc["lineItems"] as JArray;
            items = items ?? new JArray();

            items.Add(JObject.Parse(item.ToString()));

            doc["lineItems"] = items;

            return new Order(doc);
        }

        protected Order SetOrAddProperty<T>(string name, T value)
        {
            var doc = (JObject) Document.DeepClone();
            var prop = doc[name]  as JValue;

            prop = prop ?? new JValue(value);
            prop.Value = value;

            doc[name] = prop;

            return new Order(doc);
        }

        public Order SetLinePrice(string id, decimal d)
        {
            var doc = (JObject)Document.DeepClone();
            var items = Document["lineItems"];

            if (items != null)
            {
                var item = items.Children<JObject>().FirstOrDefault(x => x["id"].Value<string>()==id);
                if (item != null)
                {
                    item["price"] = d;
                }
            }

            doc["lineItems"] = items;

            return new Order(doc);
        }
    }

    public class LineItem
    {
        public LineItem()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }

        public int Quantity { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
