using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IUserService _userService;


    public ClientController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("clients")]
    public List<ClientModel> GetAllClients()
    {
        List<ClientModel> clientModels = new List<ClientModel>();
        _userService.GetAllClient().ForEach(client =>
        {
            clientModels.Add(new ClientModel(client));
        });
        return clientModels;
    }

    [HttpDelete("delclient/{clientId}")]
    public IActionResult DeleteClient(int clientId)
    {
        _userService.DeleteClient(clientId);
        return Ok("Заказчик успешно удален.");
    }

    [HttpPost("addclient")]
    public IActionResult AddClient([FromBody] AddClientModel clientModel)
    {

        Client newClient = new Client
        {
            Title = clientModel.Title,
            Info = clientModel.Info,
            Number = clientModel.Number,
            Email = clientModel.Email,
        };

        _userService.AddClient(newClient);

        return Ok("Новый заказчик успешно добавлен.");

    }
    [HttpPut("updateclient/{clientId}")]
    public IActionResult UpdateClient([FromBody] AddClientModel clientModel, int clientId)
    {
        Client updatedClient = new Client
        {
            Title = clientModel.Title,
            Info = clientModel.Info,
            Number = clientModel.Number,
            Email = clientModel.Email
        };

        _userService.UpdateClient(updatedClient, clientId);

        return Ok("Данные заказчика успешно обновлены.");
    }
}