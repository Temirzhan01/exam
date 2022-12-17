using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Classes
{
    interface IStrategy // стратегия для генерации в зависимости от цены
    {
        public string GetHonoraryCode(Product product);
    }
}
