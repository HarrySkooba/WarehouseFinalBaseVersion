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
    public partial class Products : Page, INotifyPropertyChanged
    {
        public Products()
        {
            InitializeComponent();
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public class ProductModels
        {
            public int IdProduct { get; set; }

            public string Title { get; set; }

            public string Category { get; set; }

            public string Brand { get; set; }

            public decimal Price { get; set; }

            public int Amount { get; set; }

            public int Minamount { get; set; }

            public string ProviderTitle { get; set; }

            public int Idprovider { get; set; }

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckProducts();
        }

        private async Task CheckProducts()
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

                    List<ProductModels> products = JsonConvert.DeserializeObject<List<ProductModels>>(responseContent);

                    lst.ItemsSource = products;
                }
            }
        }

        private async Task DeleteSelectedProductAsync()
        {
            if (lst.SelectedItem != null)
            {
                ProductModels selectedProduct = (ProductModels)lst.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот продукт?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int productId = selectedProduct.IdProduct;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7107/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync($"api/Product/delproduct/{productId}");

                        if (response.IsSuccessStatusCode)
                        {
                            List<ProductModels> products = (List<ProductModels>)lst.ItemsSource;
                            products.Remove(selectedProduct);
                            lst.ItemsSource = null;
                            lst.ItemsSource = products;

                            MessageBox.Show("Продукт успешно удален.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить продукт.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addproduct = new AddProduct();
            NavigationService.Navigate(addproduct);
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                ProductModels selectedProduct = (ProductModels)lst.SelectedItem;
                NavigationService.Navigate(new RedProduct(selectedProduct));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования.");
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedProductAsync();
        }
    }
}
