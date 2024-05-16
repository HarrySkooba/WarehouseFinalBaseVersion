using System.Windows;

namespace FrontEnd
{
    public partial class WindowApp2 : Window
    {
        public WindowApp2()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Products();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Provider();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Supply();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Client();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Order();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
