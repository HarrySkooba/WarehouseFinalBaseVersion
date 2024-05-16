using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;
using Backend.DB;

namespace FrontEnd
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void OnPasswordChanged(object sender, RoutedEventArgs e)

        {
            if (tb2.Password.Length > 0)
            {
                WaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                WaterMark.Visibility = Visibility.Visible;
            }
        }
        private async Task CheckAdminLoginAsync(UserLoginModel loginModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Login/loginadmin", loginModel);

   
                    if (response.IsSuccessStatusCode)
                    {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    WindowApp windowapp = new WindowApp();
                    windowapp.Show();
                    Hide();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(error, "Error");
                    }
            }
        }

        private async Task CheckLoginAsync(UserLoginModel loginModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Login/login", loginModel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    WindowApp2 windowapp = new WindowApp2();
                    windowapp.Show();
                    Hide();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Error");
                }
            }
        }

        private async void Auth_Button(object sender, RoutedEventArgs e)
        {
            if(checkb1.IsChecked == true)
            {
                UserLoginModel loginModel = new UserLoginModel
                {
                    Username = tb1.Text,
                    Password = tb2.Password,
                };
                await CheckAdminLoginAsync(loginModel);
            }
            else
            {
                UserLoginModel loginModel = new UserLoginModel
                {
                    Username = tb1.Text,
                    Password = tb2.Password,
                };
                await CheckLoginAsync(loginModel);
            }
                
        }
    }
}