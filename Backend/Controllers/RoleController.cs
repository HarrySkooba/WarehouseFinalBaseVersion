using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IUserService _userService;


    public RoleController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("roles")]
    public List<RoleModel> GetAllRoles()
    {
        List<RoleModel> roleModels = new List<RoleModel>();
        _userService.GetAllRole().ForEach(role =>
        {
            roleModels.Add(new RoleModel(role));
        });
        return roleModels;
    }

    [HttpDelete("delrole/{roleId}")]
    public IActionResult DeleteRole(int roleId)
    {
        _userService.DeleteRole(roleId);
        return Ok("Должность успешно удалена.");
    }

    [HttpPost("addrole")]
    public IActionResult AddRole([FromBody] AddRoleModel roleModel)
    {

        Role newRole = new Role
        {
            Role1 = roleModel.Role1,
        };

        _userService.AddRole(newRole);

        return Ok("Новая должность успешно добавлена.");

    }
    [HttpPut("updaterole/{roleId}")]
    public IActionResult UpdateRole([FromBody] AddRoleModel roleModel, int roleId)
    {
        Role updatedRole = new Role
        {
            Role1 = roleModel.Role1,
        };

        _userService.UpdateRole(updatedRole, roleId);

        return Ok("Данные должности успешно обновлены.");
    }
}