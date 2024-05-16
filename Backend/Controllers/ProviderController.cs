using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProviderController : ControllerBase
{
    private readonly IUserService _userService;


    public ProviderController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("providers")]
    public List<ProviderModel> GetAllProviders()
    {
        List<ProviderModel> providerModels = new List<ProviderModel>();
        _userService.GetAllProvider().ForEach(provider =>
        {
            providerModels.Add(new ProviderModel(provider));
        });
        return providerModels;
    }

    [HttpDelete("delprovider/{providerId}")]
    public IActionResult DeleteProvider(int providerId)
    {
        _userService.DeleteProvider(providerId);
        return Ok("Поставщик успешно удален.");
    }

    [HttpPost("addprovider")]
    public IActionResult AddProvider([FromBody] AddProviderModel providerModel)
    {

        Provider newProvider = new Provider
        {
            Title = providerModel.Title,
            Info = providerModel.Info,
            Number = providerModel.Number,
            Email = providerModel.Email
        };

        _userService.AddProvider(newProvider);

        return Ok("Новый поставщик успешно добавлен.");

    }
    [HttpPut("updateprovider/{providerId}")]
    public IActionResult UpdateProvider([FromBody] AddProviderModel providerModel, int providerId)
    {
        Provider updatedProvider = new Provider
        {
            Title = providerModel.Title,
            Info = providerModel.Info,
            Number = providerModel.Number,
            Email = providerModel.Email
        };

        _userService.UpdateProvider(updatedProvider, providerId);

        return Ok("Данные поставщика успешно обновлены.");
    }
}