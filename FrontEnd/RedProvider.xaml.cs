using Backend.DB;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Net.Http.Json;
using static FrontEnd.Provider;

namespace FrontEnd
{
    public partial class RedProvider : Page, INotifyPropertyChanged
    {
        private int providerId;
        public RedProvider(ProviderModels selectedProvider)
        {
            InitializeComponent();
            tb1.Text = selectedProvider.Title;
            tb2.Text = selectedProvider.Info;
            tb3.Text = selectedProvider.Email;
            tb4.Text = selectedProvider.Number;
            providerId = selectedProvider.Idprovider;
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task UpdateProviderAsync(AddProviderModel addProvidermodel)
        {
            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Provider/updateprovider/{providerId}", addProvidermodel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные поставщика успешно обновлены.");
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
                AddProviderModel addProvidermodel = new AddProviderModel
                {
                    Title = tb1.Text,
                    Info = tb2.Text,
                    Email = tb3.Text,
                    Number = tb4.Text,
                };
                await UpdateProviderAsync(addProvidermodel);
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
