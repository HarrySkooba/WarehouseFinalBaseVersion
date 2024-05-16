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
using System.Windows.Documents;

namespace FrontEnd
{
    public partial class Order : Page, INotifyPropertyChanged
    {
        public Order()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class OrderModels
        {
            public int Idorder { get; set; }

            public int Idproduct { get; set; }

            public int Idclient { get; set; }

            public string Date { get; set; }

            public int Amount { get; set; }

            public decimal Pricebyone { get; set; }

            public decimal Allprice { get; set; }

            public string Productname { get; set; } = null!;

            public string Clientname { get; set; } = null!;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckOrders();
        }

        private async Task CheckOrders()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Order/orders");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    List<OrderModels> orders = JsonConvert.DeserializeObject<List<OrderModels>>(responseContent);

                    lst.ItemsSource = orders;
                }
            }
        }

        private async Task DeleteSelectedOrderAsync()
        {
            if (lst.SelectedItem != null)
            {
                OrderModels selectedOrder = (OrderModels)lst.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int orderId = selectedOrder.Idorder;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Order/delorder/{orderId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<OrderModels> orders = (List<OrderModels>)lst.ItemsSource;
                            orders.Remove(selectedOrder);
                            lst.ItemsSource = null;
                            lst.ItemsSource = orders;

                            MessageBox.Show("Заказ успешно удалена.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить заказ.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddOrder addorder = new AddOrder();
            NavigationService.Navigate(addorder);
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                OrderModels selectedOrder = (OrderModels)lst.SelectedItem;
                NavigationService.Navigate(new RedOrder(selectedOrder));
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedOrderAsync();
        }
    }
}
