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
    public partial class AddSupply : Page, INotifyPropertyChanged
    {
        public AddSupply()
        {
            InitializeComponent();
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
            await LoadProvidersAsync();
            await LoadProductsAsync();
        }

        public async Task AddSupplyAsync(AddSupplyModel addSupplymodel)
        {

                using (var client = new HttpClient())

                {
                    client.BaseAddress = new Uri("https://localhost:7107/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Supply/addsupply", addSupplymodel);


                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Поставка добавлена!");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(error, "Error");
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

                        await AddSupplyAsync(addSupplymodel);
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
