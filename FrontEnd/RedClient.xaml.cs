using Backend.DB;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Net.Http.Json;
using static FrontEnd.Client;

namespace FrontEnd
{
    public partial class RedClient : Page, INotifyPropertyChanged
    {
        private int clientId;
        public RedClient(ClientModels selectedClient)
        {
            InitializeComponent();
            tb1.Text = selectedClient.Title;
            tb2.Text = selectedClient.Info;
            tb3.Text = selectedClient.Email;
            tb4.Text = selectedClient.Number;
            clientId = selectedClient.Idclient;
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task UpdateClientAsync(AddClientModel addClientmodel)
        {
            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Client/updateclient/{clientId}", addClientmodel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные заказчика успешно обновлены.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Ошибка при обновлении пользователя");
                }
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb1.Text) && !string.IsNullOrWhiteSpace(tb2.Text) && !string.IsNullOrWhiteSpace(tb3.Text) && !string.IsNullOrWhiteSpace(tb4.Text))
            {
                AddClientModel addClientmodel = new AddClientModel
                {
                    Title = tb1.Text,
                    Info = tb2.Text,
                    Email = tb3.Text,
                    Number = tb4.Text,
                };
                await UpdateClientAsync(addClientmodel);
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
