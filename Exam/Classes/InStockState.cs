using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Classes
{
    public class InStockState : State
    {
        public InStockState()
        {
            this.Name = "in_stock";
        }
        public override string GetName()
        {
            return "На складе";
        }
        public override void RaisePrice(Product product, int n)
        {
            Console.WriteLine("Продукт еще не участвует в торгах");
        }
        public override void SetUp(Product product)
        {
            product.state = new ForSaleState();
            Console.WriteLine("Продукт успешно участвует в торгах");
        }
        public override void SetOff(Product product)
        {
            Console.WriteLine("Нельзя снять с торгов продукт, который в них не участвует");
        }
        public override void GiveToTheWinner(Product product)
        {
            Console.WriteLine("Нельзя отдать продукт сразу со склада");
        }
    }
}
