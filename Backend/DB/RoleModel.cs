namespace Backend.DB
{
    public class RoleModel
    {
        public int Idrole{ get; set; }
        public string RoleName { get; set; }
       

        public RoleModel(Role role)
        {
            if (role != null)
            {
                Idrole = role.Id;
                RoleName = role.Role1;
            }
            else
            {
                Idrole = -1;
                RoleName = "Unknown";
            }
        }
    }
}
