namespace Backend.DB
{
    public class AddProductModel
    {
        public int Idproduct { get; set; }

        public string Title { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public decimal Price { get; set; }

        public int Amount { get; set; } 

        public int Minamount { get; set; } 

        public int Idprovider { get; set; }
    }
}
