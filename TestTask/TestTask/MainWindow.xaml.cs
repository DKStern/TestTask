using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestTask.Models;
using TestTask.ViewModels;

namespace TestTask
{
    public partial class MainWindow : Window
    {
        private Task task;
        private StockViewModel stock;

        public MainWindow()
        {
            InitializeComponent();
            task = new Task(Generate);
            stock = new StockViewModel();
            DataContext = stock;
            task.Start();
        }

        #region Logic

        /// <summary>
        /// Закрывает приложение
        /// </summary>
        private void Exit()
        {
            Close();
        }

        /// <summary>
        /// Развернуть на весь экран или вернуть к обычному размеру
        /// </summary>
        private void Maximaze()
        {
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Свернуть окно   
        /// </summary>
        private void Minimaze()
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Вычисление нового курса акций
        /// </summary>
        private void Generate()
        {
            while (true)
            {
                if (stock.StateBtn)
                {
                    stock.AddValue(stock.CurrentStock.GetNewPrice());
                    Thread.Sleep(2000);
                }
            }
        }

        #endregion Logic

        #region Window

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            Maximaze();
        }

        private void MinScreen_Click(object sender, RoutedEventArgs e)
        {
            Minimaze();            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void AvrBtn_Click(object sender, RoutedEventArgs e)
        {
            stock.ShowAvr();
            if (stock.StateAvr)
                AvrBtn.Content = "Показать средний курс";
            else
                AvrBtn.Content = "Спрятать средний курс";
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (stock.StateBtn)
            {
                stock.StateBtn = false;
                StopBtn.Content = "Старт";
            }
            else
            {
                stock.StateBtn = true;
                StopBtn.Content = "Стоп";
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            stock.Load();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StockContext())
            {
                stock.Save();
            }
        }

        #endregion Window
    }
}
