using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Exam.Classes
{
    public abstract class State
    {
        public string Name { get; set; }
        public abstract string GetName(); // это для более удобного вывода
        public abstract void RaisePrice(Product product, int n);
        public abstract void SetUp(Product product);
        public abstract void SetOff(Product product);
        public abstract void GiveToTheWinner(Product product);
    }
}
