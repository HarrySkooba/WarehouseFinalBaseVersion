namespace Backend.DB
{
    public class AddSupplyModel
    {
        public int Idsupply { get; set; }

        public int Idproduct { get; set; }

        public int Idprovider { get; set; }

        public string Date { get; set; } = null!;

        public int Amount { get; set; }

        public decimal Pricebyone { get; set; }

        public decimal Allprice { get; set; }

    }
}
