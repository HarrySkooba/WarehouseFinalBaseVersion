using Backend.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using System.ComponentModel;
using System.Net.Http.Json;

namespace FrontEnd
{

    public partial class AddRole : Page, INotifyPropertyChanged
    {
        public AddRole()
        {
            InitializeComponent();
        }

        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task AddRoleAsync(AddRoleModel addRolemodel)
        {
           
                using (var client = new HttpClient())

                {
                    client.BaseAddress = new Uri("https://localhost:7107/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Role/addrole", addRolemodel);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Должность добавлена!");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(error, "Error");
                    }
                }
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb1.Text))
            {
                AddRoleModel addRolemodel = new AddRoleModel
                {
                    Role1 = tb1.Text,
                };
                await AddRoleAsync(addRolemodel);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Не все данные введены!");
            }
        }
    }
}
