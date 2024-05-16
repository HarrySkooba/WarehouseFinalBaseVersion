namespace Backend.DB
{
    public class UserModel
    {
        public int Iduser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rolename { get; set; }
        public int Idrole { get; set; }
        public bool? Adminpanel { get; set; }

        public UserModel(User user) 
        {
            if (user != null)
            {
                Iduser = user.Id;
                Name = user.Name;
                Email = user.Email;
                Password = user.Password;
                Rolename = user.Role.Role1;
                Idrole = user.Roleid;
                Adminpanel = user.Adminpanel;
            }
            else
            {
                Iduser = -1;
                Name = "Unknown";
                Email = "Unknown";
                Password = "Unknown";
                Rolename = "Unknown";
                Idrole = -1;
                Adminpanel = null;
            }
        }
    }
}
