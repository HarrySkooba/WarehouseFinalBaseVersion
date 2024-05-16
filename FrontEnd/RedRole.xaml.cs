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
using static FrontEnd.Role;

namespace FrontEnd
{
    public partial class RedRole : Page, INotifyPropertyChanged
    {
        private int roleId;
        public RedRole(RoleModels selectedRole)
        {
            InitializeComponent();
            tb1.Text = selectedRole.RoleName;
            roleId = selectedRole.Idrole;
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;


        public class RoleModel
        {
            public int Idrole { get; set; }
            public string RoleName { get; set; }
        }

        public async Task UpdateRoleAsync(AddRoleModel addRolemodel, int roleId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Role/updaterole/{roleId}", addRolemodel);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные должности успешно обновлены.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Ошибка при обновлении должности");
                }
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb1.Text))
            {
                AddRoleModel addRolemodel = new AddRoleModel
                {
                    Role1 = tb1.Text,
                    Idrole = roleId,
                };
                await UpdateRoleAsync(addRolemodel, roleId);
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
