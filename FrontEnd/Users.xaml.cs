using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.ComponentModel;
using System;
using Backend.DB;

namespace FrontEnd
{
    public partial class Users : Page, INotifyPropertyChanged
    {
        public Users()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class UserModels
        {
            public int Iduser {  get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Rolename { get; set; }
            public bool? AdminPanel { get; set; }
            public int Idrole { get; set; }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckUsers();
        }

        private async Task CheckUsers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/User/users");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<UserModels> users = JsonConvert.DeserializeObject<List<UserModels>>(responseContent);

                    lst.ItemsSource = users;    
                }
            }
        }

        private async Task DeleteSelectedUserAsync()
        {
            if (lst.SelectedItem != null)
            {
                UserModels selectedUser = (UserModels)lst.SelectedItem;

                if (selectedUser.Rolename == "Admin")
                {
                    MessageBox.Show("Пользователя с должностью 'Admin' нельзя удалить.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int userId = selectedUser.Iduser;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/User/deluser/{userId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<UserModels> users = (List<UserModels>)lst.ItemsSource;
                            users.Remove(selectedUser);
                            lst.ItemsSource = null;
                            lst.ItemsSource = users;

                            MessageBox.Show("Пользователь успешно удален.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить пользователя.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddUser adduser = new AddUser();
            NavigationService.Navigate(adduser);
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                UserModels selectedUser = (UserModels)lst.SelectedItem;
                NavigationService.Navigate(new RedUser(selectedUser));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
           await DeleteSelectedUserAsync();
        }
    }
}
