using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace ClockWPF
{
    public partial class MainWindow : Window
    {
        Thread UpdateThread;
        Point MousePos;
        bool ChangePositionRequest = false;
        public MainWindow()
        {
            InitializeComponent();
            UpdateThread = new Thread(new ThreadStart(Update));
            UpdateThread.Start();
        }

        public void Update()
        {
            UpdateTime();
            //forgive me for that 
            this.Dispatcher.Invoke(() =>{ this.Width = DateLable.Text.Length * 35; });

            while (true)
            {
                UpdateTime();
                Thread.Sleep(1000);
            }
        }


        public void UpdateTime()
        {
            this.Dispatcher.Invoke(() =>
            {
                TimeLable.Text = DateTime.Now.ToString("HH:mm:ss");
                DateLable.Text = DateTime.Now.ToString("MMMM dd yyyy, dddd");
            });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateThread.Abort();
            this.Close();
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            if (ChangePositionRequest && MousePos != pos)
            {
                this.Top = this.Top - (MousePos.Y - pos.Y);
                this.Left = this.Left - (MousePos.X - pos.X);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MousePos = e.GetPosition(this);
            ChangePositionRequest = true;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) => ChangePositionRequest = false;

        private void Window_MouseLeave(object sender, MouseEventArgs e) => ChangePositionRequest = false;
    }
}
