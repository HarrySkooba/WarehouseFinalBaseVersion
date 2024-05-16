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

namespace FrontEnd
{
    public partial class AddProduct : Page, INotifyPropertyChanged
    {
        public AddProduct()
        {
            InitializeComponent();
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
            await LoadProviderAsync();
        }

        public async Task AddProductAsync(AddProductModel addProductmodel)
        {
            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://localhost:7107/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Product/addproduct", addProductmodel);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Продукт добавлен!");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "Error");
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

                    await AddProductAsync(addProductmodel);
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
