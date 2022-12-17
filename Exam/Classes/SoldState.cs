using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Classes
{
    public class SoldState : State
    {
        public SoldState()
        {
            this.Name = "sold";
        }
        public override string GetName()
        {
            return "Продан";
        }
        public override void RaisePrice(Product product, int n)
        {
            Console.WriteLine("Продукт уже продан");
        }
        public override void SetUp(Product product)
        {
            Console.WriteLine("Продукт уже продан");
        }
        public override void SetOff(Product product)
        {
            Console.WriteLine("Нельзя снять с торгов данный продукт");
        }
        public override void GiveToTheWinner(Product product)
        {
            Console.WriteLine("Продукт уже продан");
        }
    }
}
