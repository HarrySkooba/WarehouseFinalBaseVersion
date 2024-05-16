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
using static FrontEnd.Supply;
using static FrontEnd.Users;
using Backend;

namespace FrontEnd
{
    public partial class RedSupply : Page, INotifyPropertyChanged
    {
        private SupplyModels selectedSupply;
        private int supplyId;
        public RedSupply(SupplyModels selectedSupply)
        {
            InitializeComponent();
            tb1.Text = selectedSupply.Amount.ToString();
            tb2.Text = selectedSupply.Pricebyone.ToString();
            tb3.Text = selectedSupply.Allprice.ToString();
            cbProduct.SelectedItem = selectedSupply.Idproduct;
            cbProvider.SelectedItem = selectedSupply.Idprovider;
            cbProduct.DisplayMemberPath = selectedSupply.Productname;
            cbProvider.DisplayMemberPath = selectedSupply.Providername;
            supplyId = selectedSupply.Idsupply;
            this.selectedSupply = selectedSupply;
            if (DateTime.TryParse(selectedSupply.Date, out DateTime date))
            {
                dp.SelectedDate = date;
            }
            else
            {
                MessageBox.Show("Ошибка при преобразовании даты.");
            }
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public class ProviderModel
        {
            public int Idprovider { get; set; }
            public string Title { get; set; }
        }
        public class ProductModel
        {
            public int Idproduct { get; set; }
            public string Title { get; set; }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (selectedSupply != null)
            {
                await LoadProvidersAsync();
                await LoadProductsAsync();

                foreach (ProviderModel provider in cbProvider.Items)
                {
                    if (provider.Idprovider == selectedSupply.Idprovider)
                    {
                        cbProvider.SelectedItem = provider;
                        break;
                    }
                }
                foreach (ProductModel product in cbProduct.Items)
                {
                    if (product.Idproduct == selectedSupply.Idproduct)
                    {
                        cbProduct.SelectedItem = product;
                        break;
                    }
                }
            }
        }

        public async Task UpdateSupplyAsync(AddSupplyModel addSupplymodel)
        {

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Supply/updatesupply/{supplyId}", addSupplymodel);



                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные поставки успешно обновлены.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Ошибка при обновлении поставки");
                }
            }
        }

        private async Task LoadProvidersAsync()
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

                    List<ProviderModel> providers = JsonConvert.DeserializeObject<List<ProviderModel>>(responseContent);

                    cbProvider.DisplayMemberPath = "Title";
                    cbProvider.ItemsSource = null;
                    cbProvider.Items.Clear();
                    cbProvider.ItemsSource = providers;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить список поставщиков.");
                }
            }
        }
        private async Task LoadProductsAsync()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Product/products");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<ProductModel> products = JsonConvert.DeserializeObject<List<ProductModel>>(responseContent);

                    cbProduct.DisplayMemberPath = "Title";
                    cbProduct.ItemsSource = null;
                    cbProduct.Items.Clear();
                    cbProduct.ItemsSource = products;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить список продуктов.");
                }
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (cbProduct.SelectedItem != null && cbProvider.SelectedItem != null && !string.IsNullOrWhiteSpace(tb1.Text) && !string.IsNullOrWhiteSpace(tb2.Text) && !string.IsNullOrWhiteSpace(tb3.Text) && dp.SelectedDate != null)
            {
                ProviderModel selectedProvider = (ProviderModel)cbProvider.SelectedItem;
                ProductModel selectedProduct = (ProductModel)cbProduct.SelectedItem;
                int providerId = selectedProvider.Idprovider;
                int productId = selectedProduct.Idproduct;
                DateTime selectedDate = dp.SelectedDate ?? default(DateTime);
                DateTime currentDate = DateTime.Today;

                if (int.TryParse(tb1.Text, out int amount) &&
                    int.TryParse(tb2.Text, out int pricebyone) &&
                    int.TryParse(tb3.Text, out int allprice))
                {
                    if (selectedDate < currentDate) 
                    {
                        MessageBox.Show("Выбранная дата не может быть раньше текущей даты. Дата изменена на сегодня.");
                        dp.SelectedDate = currentDate; 
                    }
                    else
                    {
                        AddSupplyModel addSupplymodel = new AddSupplyModel
                        {
                            Amount = amount,
                            Pricebyone = pricebyone,
                            Allprice = allprice,
                            Idproduct = productId,
                            Idprovider = providerId,
                            Date = dp.SelectedDate?.ToString("yyyy-MM-dd")
                        };

                        await UpdateSupplyAsync(addSupplymodel);
                        NavigationService.GoBack();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите числовые значения для количества, цены за штуку и полной цены.");
                }
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
