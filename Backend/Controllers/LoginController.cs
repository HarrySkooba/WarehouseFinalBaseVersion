using Backend;
using Microsoft.AspNetCore.Mvc;
using Backend.DB;
using System.Diagnostics.Eventing.Reader;



[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;


    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

  [HttpPost("loginadmin")]
  public IActionResult CheckLoginAdmin([FromBody] UserLoginModel loginModel)
  {
    var user = _userService.GetUserByLogin(loginModel.Username, loginModel.Password);

        if (user != null)
        {
            if (user.Adminpanel == true)
            {
                return Ok("Вход выполнен успешно!");
            }
            else
            {
                return BadRequest("Этот пользователь не имеет доступ к админ панели!");
            }
            
        }
        else if (string.IsNullOrWhiteSpace(loginModel.Password) || string.IsNullOrWhiteSpace(loginModel.Username))
        {
            return BadRequest("Не все данные введены!");
        }
        else
        {
            return BadRequest("Неправильный логин или пароль!");
        }     
  }

    [HttpPost("login")]
    public IActionResult CheckLogin([FromBody] UserLoginModel loginModel)
    {
        var user = _userService.GetUserByLogin(loginModel.Username, loginModel.Password);

        if (user != null)
        {
            return Ok("Вход выполнен успешно!");
        }
        else if (string.IsNullOrWhiteSpace(loginModel.Password) || string.IsNullOrWhiteSpace(loginModel.Username))
        {
            return BadRequest("Не все данные введены!");
        }
        else
        {
            return BadRequest("Неправильный логин или пароль!");
        }
    }
}
