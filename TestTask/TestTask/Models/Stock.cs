using System;

namespace TestTask.Models
{
    class Stock
    {
        public double Price { get; private set; }

        public Stock(double price)
        {
            Price = price;
        }

        public Stock(Stock stock)
        {
            Price = stock.Price;
        }

        /// <summary>
        /// Получение нового курса акций
        /// </summary>
        /// <returns>Новый курс акций</returns>
        public double GetNewPrice()
        {
            Random random = new Random();
            double k = random.Next(1, 50) / 100.0;
            int help = random.Next(0, 2);
            if (help == 1)
                return Math.Round(Price * (1 + k),2);
            else
                return Math.Round(Price * (1 - k), 2);
        }
    }
}
