using Backend.DB;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Net.Http.Json;
using static FrontEnd.Products;
using static FrontEnd.Users;

namespace FrontEnd
{
    public partial class RedProduct : Page, INotifyPropertyChanged
    {
        private ProductModels selectedProduct;
        private int productId;
        public RedProduct(ProductModels selectedProduct)
        {
            InitializeComponent();
            tb1.Text = selectedProduct.Title;
            tb2.Text = selectedProduct.Category;
            tb3.Text = selectedProduct.Brand;
            tb4.Text = selectedProduct.Price.ToString();
            tb5.Text = selectedProduct.Amount.ToString();
            tb6.Text = selectedProduct.Minamount.ToString();
            cbProductProvider.SelectedItem = selectedProduct.Idprovider;
            cbProductProvider.DisplayMemberPath = selectedProduct.ProviderTitle;
            productId = selectedProduct.IdProduct;
            this.selectedProduct = selectedProduct;
        }
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;

        public class ProviderModel
        {
            public int Idprovider { get; set; }

            public string Title { get; set; }

            public string Info { get; set; }

            public string Number { get; set; }

            public string Email { get; set; }

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                await LoadProviderAsync();

                foreach (ProviderModel provider in cbProductProvider.Items)
                {
                    if (provider.Idprovider == selectedProduct.Idprovider)
                    {
                        cbProductProvider.SelectedItem = provider;
                        break;
                    }
                }
            }
        }

        public async Task UpdateProductAsync(AddProductModel addProductmodel)
        {
            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Product/updateproduct/{productId}", addProductmodel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Данные продукт успешно обновлены.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Ошибка при обновлении пользователя");
                }
            }
        }

        private async Task LoadProviderAsync()
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

                    cbProductProvider.DisplayMemberPath = "Title";
                    cbProductProvider.ItemsSource = null;
                    cbProductProvider.Items.Clear();
                    cbProductProvider.ItemsSource = providers;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить список пользователей.");
                }
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (cbProductProvider.SelectedItem != null && !string.IsNullOrWhiteSpace(tb1.Text) && !string.IsNullOrWhiteSpace(tb2.Text) && !string.IsNullOrWhiteSpace(tb3.Text) && !string.IsNullOrWhiteSpace(tb4.Text) && !string.IsNullOrWhiteSpace(tb5.Text) && !string.IsNullOrWhiteSpace(tb6.Text))
            {
                ProviderModel selectedProvider = (ProviderModel)cbProductProvider.SelectedItem;
                int ProviderId = selectedProvider.Idprovider;

                if (int.TryParse(tb4.Text, out int price) &&
                    int.TryParse(tb5.Text, out int amount) &&
                    int.TryParse(tb6.Text, out int minamount))
                {
                    AddProductModel addProductmodel = new AddProductModel
                    {
                        Title = tb1.Text,
                        Category = tb2.Text,
                        Brand = tb3.Text,
                        Price = price,
                        Amount = amount,
                        Minamount = minamount,
                        Idprovider = ProviderId
                    };

                    await UpdateProductAsync(addProductmodel);
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите числовые значения для цены, количества и минимального количества товара.");
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
