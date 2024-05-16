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
    public partial class Provider : Page, INotifyPropertyChanged
    {
        public Provider()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class ProviderModels
        {
            public int Idprovider { get; set; }

            public string Title { get; set; }

            public string Info { get; set; }

            public string Number { get; set; }

            public string Email { get; set; }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckProviders();
        }

        private async Task CheckProviders()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Provider/providers");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<ProviderModels> providers = JsonConvert.DeserializeObject<List<ProviderModels>>(responseContent);

                    lst.ItemsSource = providers;
                }
            }
        }

        private async Task DeleteSelectedProviderAsync()
        {
            if (lst.SelectedItem != null)
            {
                ProviderModels selectedProvider = (ProviderModels)lst.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого поставщика?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int providerId = selectedProvider.Idprovider;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Provider/delprovider/{providerId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<ProviderModels> providers = (List<ProviderModels>)lst.ItemsSource;
                            providers.Remove(selectedProvider);
                            lst.ItemsSource = null;
                            lst.ItemsSource = providers;

                            MessageBox.Show("Поставщик успешно удален.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить поставщика.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddProvider addprovider = new AddProvider();
            NavigationService.Navigate(addprovider);

        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                ProviderModels selectedProvider = (ProviderModels)lst.SelectedItem;
                NavigationService.Navigate(new RedProvider(selectedProvider));
            }
            else
            {
                MessageBox.Show("Выберите поставщика для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedProviderAsync();
        }
    }
}
