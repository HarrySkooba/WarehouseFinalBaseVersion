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
    public partial class AddOrder : Page, INotifyPropertyChanged
    {
        public AddOrder()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public class ClientModel
        {
            public int Idclient { get; set; }
            public string Title { get; set; }
        }
        public class ProductModel
        {
            public int Idproduct { get; set; }
            public string Title { get; set; }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadClientsAsync();
            await LoadProductsAsync();
        }

        public async Task AddOrderAsync(AddOrderModel addOrdermodel)
        {

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Order/addorder", addOrdermodel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Заказ добавлена!");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Error");
                }
            }
        }

        private async Task LoadClientsAsync()
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

                    List<ClientModel> providers = JsonConvert.DeserializeObject<List<ClientModel>>(responseContent);

                    cbClient.DisplayMemberPath = "Title";
                    cbClient.ItemsSource = null;
                    cbClient.Items.Clear();
                    cbClient.ItemsSource = providers;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить список заказчиков.");
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
            if (cbProduct.SelectedItem != null && cbClient.SelectedItem != null && !string.IsNullOrWhiteSpace(tb1.Text) && !string.IsNullOrWhiteSpace(tb2.Text) && !string.IsNullOrWhiteSpace(tb3.Text) && dp.SelectedDate != null)
            {
                ClientModel selectedClient = (ClientModel)cbClient.SelectedItem;
                ProductModel selectedProduct = (ProductModel)cbProduct.SelectedItem;
                int clientId = selectedClient.Idclient;
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
                        AddOrderModel addOrdermodel = new AddOrderModel
                        {
                            Amount = amount,
                            Pricebyone = pricebyone,
                            Allprice = allprice,
                            Idproduct = productId,
                            Idclient = clientId,
                            Date = dp.SelectedDate?.ToString("yyyy-MM-dd")
                        };

                        await AddOrderAsync(addOrdermodel);
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
