using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.ComponentModel;
using System;
using Backend.DB;
using static FrontEnd.Users;
using static FrontEnd.Products;

namespace FrontEnd
{
    public partial class Supply : Page, INotifyPropertyChanged
    {
        public Supply()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class SupplyModels
        {
            public int Idsupply { get; set; }

            public int Idproduct { get; set; }

            public int Idprovider { get; set; }

            public string Date { get; set; }

            public int Amount { get; set; }

            public decimal Pricebyone { get; set; }

            public decimal Allprice { get; set; }

            public string Productname { get; set; } = null!;

            public string Providername { get; set; } = null!;

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckSupplys();
        }

        private async Task CheckSupplys()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Supply/supplies");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<SupplyModels> supplies = JsonConvert.DeserializeObject<List<SupplyModels>>(responseContent);

                    lst.ItemsSource = supplies;
                }
            }
        }

        private async Task DeleteSelectedSupplyAsync()
        {
            if (lst.SelectedItem != null)
            {
                SupplyModels selectedSupply = (SupplyModels)lst.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту поставку?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int supplyId = selectedSupply.Idsupply;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Supply/delsupply/{supplyId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<SupplyModels> supplies = (List<SupplyModels>)lst.ItemsSource;
                            supplies.Remove(selectedSupply);
                            lst.ItemsSource = null;
                            lst.ItemsSource = supplies;

                            MessageBox.Show("Поставка успешно удалена.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить поставку.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставку для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddSupply addsupply = new AddSupply();
            NavigationService.Navigate(addsupply);
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                SupplyModels selectedSupply = (SupplyModels)lst.SelectedItem;
                NavigationService.Navigate(new RedSupply(selectedSupply));
            }
            else
            {
                MessageBox.Show("Выберите поставку для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedSupplyAsync();
        }
    }
}
