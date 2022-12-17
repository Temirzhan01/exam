using Exam.Classes;
using Newtonsoft.Json;
class Program
{
    static void Main(string[] args)
    {
        void Show(Reader r, Model m) // Метод для вывыода и в целом интерфейс
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
                    r.products[k - 1].SetUp(); // метод на аукцион
                    m.OverrideFile(r); // сохраняем
                    break;
                case 2:
                    Console.WriteLine("Введите цену которую хотите выставить: ");
                    int p = Convert.ToInt32(Console.ReadLine());
                    r.products[k - 1].RaisePrice(p); // обновляем цену
                    m.OverrideFile(r);
                    break;
                case 3:
                    r.products[k - 1].GiveToTheWinner(); // отдаем победителю
                    m.OverrideFile(r);
                    break;
                case 4:
                    r.products[k - 1].SetOff(); // снимаем с аукциона
                    m.OverrideFile(r);
                    break;
            }
        }
        Reader reader = new Reader(); // читает из файла
        Model model = new Model(); // модель которая обновляет файл
        reader.GetProducts(); // метод который вытаскивает продукты из файла или же из себя
        int i = 1;
        while (i != 0 ) // для бесконечного цикла 
        {
            Show(reader, model); // запускаем меню, типа можно до бесконечности смотреть и менять состояния товаров и т.д.
            Console.WriteLine("1) Закрыть? \n2) Продолжить :");
            string j = Console.ReadLine();
            if (j == "1") { break; }
        }
    }
}