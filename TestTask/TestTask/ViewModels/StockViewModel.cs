using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestTask.Models;

namespace TestTask.ViewModels
{
    class StockViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Stock averageStock;
        private Stock currentStock;
        private bool isLess;
        private string stateAvr = "Показать средний курс";
        private string stateBtn = "Стоп";

        public Stock CurrentStock
        {
            get
            {
                return currentStock;
            }
            set
            {
                currentStock = value;
                NotifyPropertyChanged("CurrentStock");
            }
        }
        public Stock AverageStock
        {
            get
            {
                return averageStock;
            }
            set
            {
                averageStock = value;
                NotifyPropertyChanged("AverageStock");
            }
        }
        public bool IsLess
        {
            get
            {
                return isLess;
            }
            set
            {
                isLess = value;
                NotifyPropertyChanged("IsLess");
            }
        }
        public string StateAvr
        {
            get
            {
                return stateAvr;
            }
            set
            {
                stateAvr = value;
                NotifyPropertyChanged();
            }
        }
        public string StateBtn
        {
            get
            {
                return stateBtn;
            }
            set
            {
                stateBtn = value;
                NotifyPropertyChanged();
            }
        }

        public SeriesCollection Series { get; private set; }
        public List<Stock> history { get; }

        public void InitSeries()
        {
            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Курс акций",
                    Values = new ChartValues<double> {currentStock.Price}
                }
            };
        }

        public StockViewModel()
        {
            currentStock = new Stock(100.0);
            averageStock = new Stock(0.0);
            history = new List<Stock>();
            InitSeries();
        }

        public StockViewModel(List<Stock> _history, Stock _cur, Stock _avr)
        {
            currentStock = new Stock(_cur);
            averageStock = new Stock(_avr);
            history = new List<Stock>(_history);
            InitSeries();
        }

        /// <summary>
        /// Вычисление среднего курса акций
        /// </summary>
        /// <returns>Средний курс акций</returns>
        private double GetAvr()
        {
            double avr = 0;
            foreach(var i in history)
            {
                avr += i.Price;
            }
            return Math.Round(avr / history.Count, 2);
        }

        /// <summary>
        /// Добавляет новые значение 
        /// </summary>
        /// <param name="newValue">Новое значение</param>
        public void AddValue(double newValue)
        {
            history.Add(CurrentStock);
            CurrentStock = new Stock(newValue);
            AverageStock = new Stock(GetAvr());
            Series[0].Values.Add(newValue);
            if (newValue < AverageStock.Price)
                IsLess = true;
            else
                IsLess = false;
        }
    }
}
