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
using static FrontEnd.Order;

namespace FrontEnd
{
    public partial class RedOrder : Page, INotifyPropertyChanged
    {
        private OrderModels selectedOrder;
        private int orderId;
        public RedOrder(OrderModels selectedOrder)
        {
            InitializeComponent();
            tb1.Text = selectedOrder.Amount.ToString();
            tb2.Text = selectedOrder.Pricebyone.ToString();
            tb3.Text = selectedOrder.Allprice.ToString();
            cbProduct.SelectedItem = selectedOrder.Idproduct;
            cbClient.SelectedItem = selectedOrder.Idclient;
            cbProduct.DisplayMemberPath = selectedOrder.Productname;
            cbClient.DisplayMemberPath = selectedOrder.Clientname;
            orderId = selectedOrder.Idorder;
            this.selectedOrder = selectedOrder;
            if (DateTime.TryParse(selectedOrder.Date, out DateTime date))
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
            if (selectedOrder != null)
            {
                await LoadClientsAsync();
                await LoadProductsAsync();

                foreach (ClientModel client in cbClient.Items)
                {
                    if (client.Idclient == selectedOrder.Idclient)
                    {
                        cbClient.SelectedItem = client;
                        break;
                    }
                }
                foreach (ProductModel product in cbProduct.Items)
                {
                    if (product.Idproduct == selectedOrder.Idproduct)
                    {
                        cbProduct.SelectedItem = product;
                        break;
                    }
                }
            }
        }

        public async Task UpdateOrderAsync(AddOrderModel addOrdermodel)
        {

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Order/updateorder/{orderId}", addOrdermodel);



                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные заказа успешно обновлены.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Ошибка при обновлении заказа");
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

                    List<ClientModel> clients = JsonConvert.DeserializeObject<List<ClientModel>>(responseContent);

                    cbClient.DisplayMemberPath = "Title";
                    cbClient.ItemsSource = null;
                    cbClient.Items.Clear();
                    cbClient.ItemsSource = clients;
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

                        await UpdateOrderAsync(addOrdermodel);
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
