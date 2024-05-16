namespace Backend.DB
{
    public class AddClientModel
    {
        public int Idclient { get; set; }

        public string Title { get; set; } = null!;

        public string Info { get; set; } = null!;

        public string Number { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
