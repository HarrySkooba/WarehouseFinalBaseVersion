using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.ComponentModel;
using System;
using Backend.DB;
using static FrontEnd.Client;

namespace FrontEnd
{
    public partial class Client : Page, INotifyPropertyChanged
    {
        public Client()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class ClientModels
        {
            public int Idclient { get; set; }

            public string Title { get; set; } = null!;

            public string Info { get; set; } = null!;

            public string Number { get; set; } = null!;

            public string Email { get; set; } = null!;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckClients();
        }

        private async Task CheckClients()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Client/clients");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<ClientModels> clients = JsonConvert.DeserializeObject<List<ClientModels>>(responseContent);

                    lst.ItemsSource = clients;
                }
            }
        }

        private async Task DeleteSelectedClientAsync()
        {
            if (lst.SelectedItem != null)
            {
                ClientModels selectedClient = (ClientModels)lst.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого заказчика?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int clientId = selectedClient.Idclient;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Client/delclient/{clientId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<ClientModels> clients = (List<ClientModels>)lst.ItemsSource;
                            clients.Remove(selectedClient);
                            lst.ItemsSource = null;
                            lst.ItemsSource = clients;

                            MessageBox.Show("Заказчик успешно удален.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить заказчика.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказчика для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddClient addclient = new AddClient();
            NavigationService.Navigate(addclient);

        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                ClientModels selectedClient = (ClientModels)lst.SelectedItem;
                NavigationService.Navigate(new RedClient(selectedClient));
            }
            else
            {
                MessageBox.Show("Выберите заказчика для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedClientAsync();
        }
    }
}
