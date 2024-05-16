using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.DB
{
    public class OrderModel
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

        public OrderModel(Order order)
        {
            if (order != null)
            {
                Idorder = order.Id;
                Idproduct = order.Productid;
                Idclient = order.Clientid;
                Date = order.Date;
                Amount = order.Amount;
                Pricebyone = order.Pricebyone;
                Allprice = order.Allprice;
                Productname = order.Product != null ? order.Product.Title : null;
                Clientname = order.Client != null ? order.Client.Title : null;

            }
            else
            {
                Idorder = -1;
                Idproduct = -1;
                Idclient = -1;
                Amount = 0;
                Pricebyone = 0;
                Allprice = 0;
                Productname = "Unknown";
                Clientname = "Unknown";
            }
        }
    }
}
