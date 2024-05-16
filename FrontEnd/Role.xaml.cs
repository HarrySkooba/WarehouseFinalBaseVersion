using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.ComponentModel;
using System;
using Backend.DB;
using static FrontEnd.Users;

namespace FrontEnd
{
    public partial class Role : Page, INotifyPropertyChanged
    {
        public Role()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class RoleModels
        {
            public int Idrole { get; set; }
            public string RoleName { get; set; }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckRoles();
        }

        private async Task CheckRoles()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Role/roles");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<RoleModels> roles = JsonConvert.DeserializeObject<List<RoleModels>>(responseContent);

                    lst.ItemsSource = roles;
                }
            }
        }

        private async Task DeleteSelectedRoleAsync()
        {
            if (lst.SelectedItem != null)
            {
                RoleModels selectedRole = (RoleModels)lst.SelectedItem;

                if (selectedRole.RoleName == "Admin")
                {
                    MessageBox.Show("Должность 'Admin' нельзя удалить.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту должность?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int roleId = selectedRole.Idrole;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Role/delrole/{roleId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<RoleModels> roles = (List<RoleModels>)lst.ItemsSource;
                            roles.Remove(selectedRole);
                            lst.ItemsSource = null;
                            lst.ItemsSource = roles;

                            MessageBox.Show("Должность успешно удалена.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить должность.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите должность для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddRole addrole = new AddRole();
            NavigationService.Navigate(addrole);
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                RoleModels selectedRole = (RoleModels)lst.SelectedItem;
                NavigationService.Navigate(new RedRole(selectedRole));
            }
            else
            {
                MessageBox.Show("Выберите должность для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedRoleAsync();
        }
    }
}
