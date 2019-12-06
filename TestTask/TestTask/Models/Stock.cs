using System;

namespace TestTask.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public Stock()
        { }

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
