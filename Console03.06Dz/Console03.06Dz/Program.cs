using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console03._06Dz
{
    class Worker
    {
        private int _forteit;
        private int _tip;

        public int Score { get; private set; }

        public Worker()
        {
            Score = 0;
            _forteit = -250;
            _tip = 100;
        }

        public void ZeroingScore()
        {
            Score = 0;
        }

        public void TakeForfeit()
        {
            Score = _forteit;
        }

        public void Repair(Client client, Storage storage)
        {
            switch (client.Problem)
            {
                case 1:
                    Score += storage.PriceСommon * 25 + _tip;
                    break;
                case 2:
                    Score += storage.PriceСommon * 15 + _tip;
                    Score += storage.PriceUnusual * 10 + _tip;
                    break;
                case 3:
                    Score += storage.PriceСommon * 10 + _tip;
                    Score += storage.PriceUnusual * 5 + _tip;
                    Score += storage.PriceRare * 5 + _tip;
                    break;
            }

            storage.SpendDetails(client, this);
            client.TakeProblem();
        }
    }

    class Client
    {
        private static Random Rnd;

        public int Problem { get; private set; }

        static Client()
        {
            Rnd = new Random();
        }

        public Client()
        {
            Problem = 1;
        }

        public void TakeProblem()
        {
            Problem = Rnd.Next(1, 3 + 1);
        }
    }

    class Storage
    {
        public int Money { get; private set; }
        public int CommonDetails { get; private set; }
        public int PriceСommon { get; private set; }
        public int UnusualDetails { get; private set; }
        public int PriceUnusual { get; private set; }
        public int RareDetails { get; private set; }
        public int PriceRare { get; private set; }

        public Storage()
        {
            Money = 1500;
            CommonDetails = 150;
            PriceСommon = 5;
            UnusualDetails = 100;
            PriceUnusual = 10;
            RareDetails = 50;
            PriceRare = 15;
        }

        public void TakeMoney(Worker worker)
        {
            Money += worker.Score;
        }

        public void SpendDetails(Client client, Worker worker)
        {
            switch (client.Problem)
            {
                case 1:
                    if (CommonDetails > 25)
                    {
                        CommonDetails -= 25;
                        Console.WriteLine("Починка машины...");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает деталей!");
                        worker.ZeroingScore();
                        worker.TakeForfeit();
                    }
                    break;
                case 2:
                    if (CommonDetails > 15 && UnusualDetails > 10)
                    {
                        CommonDetails -= 15;
                        UnusualDetails -= 10;
                        Console.WriteLine("Починка машины...");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает деталей!");
                        worker.ZeroingScore();
                        worker.TakeForfeit();
                    }
                    break;
                case 3:
                    if (CommonDetails > 10 && UnusualDetails > 5 && RareDetails > 5)
                    {
                        CommonDetails -= 10;
                        UnusualDetails -= 5;
                        RareDetails -= 5;
                        Console.WriteLine("Починка машины...");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает деталей!");
                        worker.ZeroingScore();
                        worker.TakeForfeit();
                    }
                    break;
            }
        }

        public void BuyDetails()
        {
            Console.WriteLine("Выберите тип деталей:\n1)Обычные - 5 руб за шт.\n2)Необычные - 10 руб за шт.\n3)Редкие - 15 руб за шт.");
            int typeDetails = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.Write("Выберите кол-во: ");
            int countDetails = int.Parse(Console.ReadLine());
            Console.Clear();

            switch (typeDetails)
            {
                case 1:
                    if (Money - countDetails * PriceСommon > 0)
                    {
                        CommonDetails += countDetails;
                        Money -= countDetails * PriceСommon;
                        Console.WriteLine("Детали куплены!");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает денег!");
                    }
                    break;
                case 2:
                    if (Money - countDetails * PriceUnusual > 0)
                    {
                        UnusualDetails += countDetails;
                        Money -= countDetails * PriceUnusual;
                        Console.WriteLine("Детали куплены!");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает денег!");
                    }
                    break;
                case 3:
                    if (Money - countDetails * PriceRare > 0)
                    {
                        RareDetails += countDetails;
                        Money -= countDetails * PriceRare;
                        Console.WriteLine("Детали куплены!");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает денег!");
                    }
                    break;
            }
        }
    }

    class Program
    {
        static void ShowInfo(Storage storage)
        {
            Console.SetCursorPosition(105, 0);
            Console.WriteLine($"Деньги: {storage.Money}");
            Console.SetCursorPosition(105, 1);
            Console.WriteLine($"Обычные: {storage.CommonDetails}");
            Console.SetCursorPosition(105, 2);
            Console.WriteLine($"Необычные: {storage.UnusualDetails}");
            Console.SetCursorPosition(105, 3);
            Console.WriteLine($"Редкие: {storage.RareDetails}");
            Console.SetCursorPosition(0, 0);
        }

        static void Main(string[] args)
        {
            Worker worker = new Worker();
            Client client = new Client();
            Storage storage = new Storage();

            bool isExit = false;

            while (isExit == false)
            {
                Console.Clear();
                ShowInfo(storage);
                Console.WriteLine("1)Взять заказ\n2)Купить детали\n3)Выйти");
                int menu = int.Parse(Console.ReadLine());
                Console.Clear();

                switch (menu)
                {
                    case 1:
                        Console.WriteLine($"Уровень починки - {client.Problem}");
                        Console.Write("Взять заказ? ");
                        string answer = Console.ReadLine();
                        Console.Clear();
                        if (answer == "Да")
                        {
                            worker.Repair(client, storage);
                        }
                        else
                        {
                            worker.TakeForfeit();
                            Console.WriteLine("Штраф получен!");
                        }
                        Console.ReadKey();
                        Console.Clear();
                        ShowInfo(storage);
                        Console.WriteLine($"Чек: {worker.Score} руб");
                        Console.ReadKey();
                        storage.TakeMoney(worker);
                        worker.ZeroingScore();
                        Console.Clear();
                        break;
                    case 2:
                        storage.BuyDetails();
                        break;
                    case 3:
                        isExit = true;
                        break;
                }
            }
        }
    }
}
