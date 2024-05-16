using Backend.DB;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Rewrite;
using static FrontEnd.Users;

namespace FrontEnd
{
    public partial class AddUser : Page, INotifyPropertyChanged
    {
        public AddUser()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public class RoleModel
        {
            public int Idrole { get; set; }
            public string RoleName { get; set; }
        }
   
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRolesAsync();
        }

        public async Task AddUserAsync(AddUserModel addUsermodel)
        {
            if (cbUserRole.SelectedItem != null)
            {
                using (var client = new HttpClient())

                {
                    client.BaseAddress = new Uri("https://localhost:7107/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/User/adduser", addUsermodel);


                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Пользователь добавлен!");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(error, "Error");
                    }
                }
            }
        }

        private async Task LoadRolesAsync()
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

                    List<RoleModel> roles = JsonConvert.DeserializeObject<List<RoleModel>>(responseContent);

                    cbUserRole.DisplayMemberPath = "RoleName";
                    cbUserRole.ItemsSource = null;
                    cbUserRole.Items.Clear();
                    cbUserRole.ItemsSource = roles;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить список пользователей.");
                }
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (cbUserRole.SelectedItem != null && !string.IsNullOrWhiteSpace(tb1.Text) && !string.IsNullOrWhiteSpace(tb2.Text) && !string.IsNullOrWhiteSpace(tb3.Text))
            {
                RoleModel selectedRole = (RoleModel)cbUserRole.SelectedItem;
                int roleId = selectedRole.Idrole;
                AddUserModel addUsermodel = new AddUserModel
                {
                    Name = tb1.Text,
                    Email = tb2.Text,
                    Password = tb3.Text,
                    Idrole = roleId,
                    Adminpanel = checkb1.IsChecked.HasValue ? checkb1.IsChecked.Value : false
                };
                await AddUserAsync(addUsermodel);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Не все данные введены!");
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.GoBack();
        }
    }
}
