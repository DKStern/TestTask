using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using TestTask.Models;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

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
        private bool stateAvr = false;
        private bool stateBtn = false;
        private bool isUpdating = false;

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
        public bool StateAvr
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
        public bool StateBtn
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
        public List<Stock> History { get; set; }

        /// <summary>
        /// Создаёт график курса акций
        /// </summary>
        private void InitSeries()
        {
            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Курс акций",
                    Values = new ChartValues<double>()
                }
            };
        }

        /// <summary>
        /// Создаёт график среднего курса акций
        /// </summary>
        private void CreateAvr()
        {
            var line = new LineSeries
            {
                Title = "Средний курс",
                Values = new ChartValues<double>(),
                Stroke = Brushes.Blue
            };
            Series.Add(line);
            for (int i = 0; i < Series[0].Values.Count; i++)
            {
                Series[1].Values.Add(AverageStock.Price);
            }
        }

        /// <summary>
        /// Переключение отображения среднего значения курса акций
        /// </summary>
        public void ShowAvr()
        {
            if (stateAvr)
            {
                Series.RemoveAt(1);
                stateAvr = false;
            }
            else
            {
                CreateAvr();
                stateAvr = true;
            }
        }

        public StockViewModel()
        {
            CurrentStock = new Stock(100.0);
            AverageStock = new Stock(0.0);
            History = new List<Stock>();
            History.Add(currentStock);
            InitSeries();
            Series[0].Values.Add(CurrentStock.Price);
        }

        public StockViewModel(List<Stock> _history, Stock _cur, Stock _avr)
        {
            currentStock = new Stock(_cur);
            averageStock = new Stock(_avr);
            History = new List<Stock>(_history);
            InitSeries();
        }

        /// <summary>
        /// Добавляет новые значение 
        /// </summary>
        /// <param name="newValue">Новое значение</param>
        public void AddValue(double newValue)
        {
            CurrentStock = new Stock(newValue);
            History.Add(CurrentStock);
            var avr = Math.Round(History.Average(x => x.Price), 2);
            AverageStock = new Stock(avr);
            Series[0].Values.Add(newValue);
            if (stateAvr)
            {
                Series[1].Values.Clear();
                for (int i = 0; i < Series[0].Values.Count; i++)
                {
                    Series[1].Values.Add(avr);
                }
            }
            if (newValue < AverageStock.Price)
                IsLess = true;
            else
                IsLess = false;
        }

        /// <summary>
        /// Сохранение данных в БД
        /// </summary>
        public void Save()
        {
            if (!isUpdating)
            {
                isUpdating = true;
                Task task = Task.Run(() =>
                {
                    using (var db = new StockContext())
                    {
                        db.Stocks.RemoveRange(db.Stocks);
                        foreach (var i in History)
                        {
                            db.Stocks.Add(i);
                        }
                        db.SaveChanges();
                    }
                    isUpdating = false;
                });
            }
        }

        /// <summary>
        /// Загрузка данных из БД
        /// </summary>
        public void Load()
        {
            var state = StateBtn;
            if (!isUpdating)
            {
                isUpdating = true;
                StateBtn = false;
                using (var db = new StockContext())
                {
                    History = new List<Stock>(db.Stocks);
                    Series[0].Values.Clear();
                    if (Series[1] != null)
                        Series.RemoveAt(1);
                    var list = History.Select(x => (object)x.Price).ToList();
                    Series[0].Values.AddRange(list);
                    CurrentStock = History.Last();
                    AverageStock = new Stock(Math.Round(History.Average(x => x.Price),2));
                    if (StateAvr)
                        CreateAvr();

                    isUpdating = false;
                    StateBtn = state;
                }
            }
        }
    }
}
