using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SupplyController : ControllerBase
{
    private readonly IUserService _userService;


    public SupplyController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("supplies")]
    public List<SupplyModel> GetAllSupplies()
    {
        List<SupplyModel> supplyModel = new List<SupplyModel>();
        _userService.GetAllSupply().ForEach(supply =>
        {
            supplyModel.Add(new SupplyModel(supply));
        });
        return supplyModel;
    }

    [HttpDelete("delsupply/{supplyId}")]
    public IActionResult DeleteSupply(int supplyId)
    {
        _userService.DeleteSupply(supplyId);
        return Ok("Поставка успешно удалена.");
    }

    [HttpPost("addsupply")]
    public IActionResult AddSupply([FromBody] AddSupplyModel supplyModel)
    {

        Supply newSupply = new Supply
        {
            Providerid = supplyModel.Idprovider,
            Productid = supplyModel.Idproduct,
            Date = supplyModel.Date,
            Amount = supplyModel.Amount,
            Pricebyone = supplyModel.Pricebyone,
            Allprice = supplyModel.Allprice
        };

        _userService.AddSupply(newSupply);

        return Ok("Новая поставка успешно добавлена.");

    }

    [HttpPut("updatesupply/{supplyId}")]
    public IActionResult UpdateSupply([FromBody] AddSupplyModel supplyModel, int supplyId)
    {
        Supply updatedSupply = new Supply
        {
            Providerid = supplyModel.Idprovider,
            Productid = supplyModel.Idproduct,
            Date = supplyModel.Date,
            Amount = supplyModel.Amount,
            Pricebyone = supplyModel.Pricebyone,
            Allprice = supplyModel.Allprice
        };

        _userService.UpdateSupply(updatedSupply, supplyId);

        return Ok("Данные поставки успешно обновлены.");
    }
}