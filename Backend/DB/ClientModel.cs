namespace Backend.DB
{
    public class ClientModel
    {
        public int Idclient { get; set; }

        public string Title { get; set; } = null!;

        public string Info { get; set; } = null!;

        public string Number { get; set; } = null!;

        public string Email { get; set; } = null!;

        public ClientModel(Client client)
        {
            if (client != null)
            {
                Idclient = client.Id;
                Title = client.Title;
                Info = client.Info;
                Number = client.Number;
                Email = client.Email;
            }
            else
            {
                Idclient = -1;
                Title = "Unknown";
                Info = "Unknown";
                Number = "Unknown";
                Email = "Unknown";
            }
        }
    }
}
