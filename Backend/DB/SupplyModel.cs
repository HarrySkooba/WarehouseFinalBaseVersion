namespace Backend.DB
{
    public class SupplyModel
    {
        public int Idsupply { get; set; }

        public int Idproduct { get; set; }

        public int Idprovider { get; set; }

        public string Date { get; set; } = null!;

        public int Amount { get; set; }

        public decimal Pricebyone { get; set; }

        public decimal Allprice { get; set; }

        public string Productname { get; set; } = null!;
        public string Providername { get; set; } = null!;

        public SupplyModel(Supply supply)
        {
            if (supply != null)
            {
                Idsupply = supply.Id;
                Idproduct = supply.Productid;
                Idprovider = supply.Providerid;
                Date = supply.Date;
                Amount = supply.Amount;
                Pricebyone = supply.Pricebyone;
                Allprice = supply.Allprice;
                Productname = supply.Product != null ? supply.Product.Title : null;
                Providername = supply.Provider != null ? supply.Provider.Title : null;

            }
            else
            {
                Idsupply = -1; 
                Idproduct = -1;
                Idprovider = -1;
                Date = "Unknown";
                Amount = 0;
                Pricebyone = 0;
                Allprice = 0;
                Productname = "Unknown";
                Providername = "Unknown";
            }
        }
    }
}
