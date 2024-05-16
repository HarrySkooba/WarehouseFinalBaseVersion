using System.Windows;

namespace FrontEnd
{
    public partial class WindowApp : Window
    {
        public WindowApp()
        {
            InitializeComponent();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Users();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Role();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
