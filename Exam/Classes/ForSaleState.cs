using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Classes
{
    public class ForSaleState : State
    {
        public ForSaleState()
        {
            this.Name = "for_sale";
        }
        public override string GetName()
        {
            return "На торгах";
        }
        public override void RaisePrice(Product product, int n)
        {
            product.Price = n;
            Console.WriteLine("Цена на продукт успешно поднята");
        }
        public override void SetUp(Product product)
        {
            Console.WriteLine("Продукт не может быть повторно выставлен на торги");
        }
        public override void SetOff(Product product)
        {
            product.state = new InStockState();
            Console.WriteLine("Продукт перемещен на склад");
        }
        public override void GiveToTheWinner(Product product)
        {
            if (product.Price <= 0)
            {
                Console.WriteLine("Нельзя отдать продукт бесплатно");
            }
            else
            {
                product.state = new SoldState();
                Console.WriteLine("Продукт успешно передан победителю");
                IStrategy strategy;
                if (product.Price >= 1000)
                {
                    strategy = new Gold();
                    product.HonoraryCode = Convert.ToInt32(strategy.GetHonoraryCode(product));
                }
                if (product.Price <= 1000 && product.Price >= 500)
                {
                    strategy = new Silver();
                    product.HonoraryCode = Convert.ToInt32(strategy.GetHonoraryCode(product));
                }
                if (product.Price < 500)
                {
                    strategy = new Bronze();
                    product.HonoraryCode = Convert.ToInt32(strategy.GetHonoraryCode(product));
                }
                Console.WriteLine("Ваш код = {0}", product.HonoraryCode);

            }

        }
    }
}
