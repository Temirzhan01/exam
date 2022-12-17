using Exam.Classes;
using Newtonsoft.Json;
class Program
{
    static void Main(string[] args)
    {
        void Show(Reader r, Model m)
        {
            Console.WriteLine("#  |\t Продукт");
            foreach (Product product in r.products)
            {
                Console.WriteLine("#" + (product.Id) + " |\t" + product.Name);
            }
            int k;
            Console.WriteLine("Введите номер продукта: ");
            k = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Продукт: " + r.products[k - 1].Name + " Цена: " + r.products[k - 1].Price + " Состояние: " + r.products[k - 1].state.GetName());
            Console.WriteLine("Выберите действие:\n1) Выставить на аукцион \n2) Поднять цену \n3) Выдать победителю \n4) Снять с торгов \nВведите номер:");
            int n = Convert.ToInt32(Console.ReadLine());
            switch (n)
            {
                case 1:
                    r.products[k - 1].SetUp();
                    m.OverrideFile(r);
                    break;
                case 2:
                    Console.WriteLine("Введите цену которую хотите выставить: ");
                    int p = Convert.ToInt32(Console.ReadLine());
                    r.products[k - 1].RaisePrice(p);
                    m.OverrideFile(r);
                    break;
                case 3:
                    r.products[k - 1].GiveToTheWinner();
                    m.OverrideFile(r);
                    break;
                case 4:
                    r.products[k - 1].SetOff();
                    m.OverrideFile(r);
                    break;
            }
        }
        Reader reader = new Reader();
        Model model = new Model();
        reader.GetProducts();
        Show(reader, model);
    }
}