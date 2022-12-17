using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exam.Classes
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int HonoraryCode { get; set; }
        public State state { get; set; }
        public Product(int Id, string Name, int Price, State state)
        {
            this.Id = Id;
            this.Name = Name;
            this.Price = Price;
            this.state = state;
        }
        public void RaisePrice(int n)
        {
            this.state.RaisePrice(this, n);
        }
        public void SetUp()
        {
            this.state.SetUp(this);
        }
        public void SetOff()
        {
            this.state.SetOff(this);
        }
        public void GiveToTheWinner()
        {
            this.state.GiveToTheWinner(this);
        }
    }
}

