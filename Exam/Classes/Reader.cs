using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Exam.Classes
{
    public class Reader
    {
        public List<Product> products { get; set; }
        public Reader()
        {
            products = new List<Product>();
        }
        public List<Product> GetProducts()
        {
            if (products.Any())
            {
                return products;
            }
            else
            {
                    Reader r = Newtonsoft.Json.JsonConvert.DeserializeObject<Reader>(File.ReadAllText(@"C:\Users\data.json"), new Newtonsoft.Json.JsonSerializerSettings
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    });
                    products = r.products;
                    return products;
            }
        }
    }
}
