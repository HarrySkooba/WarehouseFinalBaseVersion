namespace Backend.DB
{
    public class AddOrderModel
    {
        public int Idorder { get; set; }

        public int Idproduct { get; set; }

        public int Idclient { get; set; }

        public string Date { get; set; }

        public int Amount { get; set; }

        public decimal Pricebyone { get; set; }

        public decimal Allprice { get; set; }

    }
}
