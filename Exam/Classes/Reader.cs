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
                using (StreamReader file = File.OpenText(@"C:\Users\data.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Reader reader = (Reader)serializer.Deserialize(file, typeof(Reader));
                    products = reader.products;
                    foreach (Product i in products) 
                    {
                        if (i.state_name == "На складе") 
                        {
                            i.state = new InStockState();
                        }
                        else if (i.state_name == "На торгах")
                        {
                            i.state = new ForSaleState();
                        }
                        else if (i.state_name == "Продан")
                        {
                            i.state = new SoldState();
                        }
                    }
                    return products;
                }
            }
        }
    }
}
