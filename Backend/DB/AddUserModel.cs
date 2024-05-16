namespace Backend.DB
{
    public class AddUserModel
    {
        public int Iduser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Idrole { get; set; }
        public bool? Adminpanel { get; set; }
    }
}
