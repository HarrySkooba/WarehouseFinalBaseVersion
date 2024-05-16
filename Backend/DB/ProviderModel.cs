namespace Backend.DB
{
    public class ProviderModel
    {
        public int Idprovider { get; set; }

        public string Title { get; set; } 

        public string Info { get; set; } 

        public string Number { get; set; } 

        public string Email { get; set; }

        public ProviderModel(Provider provider)
        {
            if (provider != null)
            {
                Idprovider = provider.Id;
                Title = provider.Title;
                Info = provider.Info;
                Number = provider.Number;
                Email = provider.Email;
            }
            else
            {
                Idprovider = -1;
                Title = "Unknown";
                Info = "Unknown";
                Number = "Unknown";
                Email = "Unknown";
            }
        }
    }
}
