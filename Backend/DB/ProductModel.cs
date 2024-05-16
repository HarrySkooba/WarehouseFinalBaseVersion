namespace Backend.DB
{
    public class ProductModel
    {
        public int IdProduct { get; set; }

        public string Title { get; set; } 

        public string Category { get; set; } 

        public string Brand { get; set; } 

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public int Minamount { get; set; }
        public string ProviderTitle { get; set; } = null!;

        public int Idprovider { get; set; }

        public ProductModel(Product product)
        {
            if (product != null)
            {
                IdProduct = product.Id;
                Title = product.Title;
                Category = product.Category;
                Brand = product.Brand;
                Price = product.Price;
                Amount = product.Amount;
                Minamount = product.Minamount;
                ProviderTitle = product.Provider != null ? product.Provider.Title : null;
                Idprovider = product.Providerid;
            }
            else
            {
                IdProduct = -1;
                Title = "Unknown";
                Category = "Unknown";
                Brand = "Unknown";
                Price = 0;
                Amount = 0;
                Minamount = 0;
                ProviderTitle = "Unknown";
                Idprovider = -1;
            }
        }
    }
}
