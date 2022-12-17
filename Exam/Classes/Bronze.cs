using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Classes
{
    public class Bronze : IStrategy
    {
        public string GetHonoraryCode(Product product)
        {
            Generator gr = new Generator();
            return gr.CalculateMD5Hash("Bronze" + product.Id);
        }
    }
}
