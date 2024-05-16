using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;


    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public List<UserModel> GetAllUsers()
    {
        List<UserModel> userModels = new List<UserModel>();
        _userService.GetAllUser().ForEach(user =>
        {
            userModels.Add(new UserModel(user));
        });
        return userModels;
    }

    [HttpDelete("deluser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        _userService.DeleteUser(userId);
        return Ok("Пользователь успешно удален.");
    }

    [HttpPost("adduser")]
    public IActionResult AddUser([FromBody] AddUserModel userModel)
    {

            User newUser = new User
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Password = userModel.Password,
                Roleid = userModel.Idrole,
                Adminpanel = userModel.Adminpanel,
            };

            _userService.AddUser(newUser);

            return Ok("Новый пользователь успешно добавлен.");
      
    }

    [HttpPut("updateuser/{userId}")]
    public IActionResult UpdateUser([FromBody] AddUserModel userModel, int userId)
    {
        User updatedUser = new User
        {
            Name = userModel.Name,
            Email = userModel.Email,
            Password = userModel.Password,
            Roleid = userModel.Idrole,
            Adminpanel = userModel.Adminpanel,
        };

        _userService.UpdateUser(updatedUser, userId);

        return Ok("Данные пользователя успешно обновлены.");
    }
}