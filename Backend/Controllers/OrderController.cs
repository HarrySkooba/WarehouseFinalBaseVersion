using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IUserService _userService;


    public OrderController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("orders")]
    public List<OrderModel> GetAllOrders()
    {
        List<OrderModel> orderModel = new List<OrderModel>();
        _userService.GetAllOrder().ForEach(order =>
        {
            orderModel.Add(new OrderModel(order));
        });
        return orderModel;
    }

    [HttpDelete("delorder/{orderId}")]
    public IActionResult DeleteOrder(int orderId)
    {
        _userService.DeleteOrder(orderId);
        return Ok("Заказ успешно удален.");
    }

    [HttpPost("addorder")]
    public IActionResult AddOrder([FromBody] AddOrderModel orderModel)
    {

        Order newOrder = new Order
        {
            Clientid = orderModel.Idclient,
            Productid = orderModel.Idproduct,
            Date = orderModel.Date,
            Amount = orderModel.Amount,
            Pricebyone = orderModel.Pricebyone,
            Allprice = orderModel.Allprice
        };

        _userService.AddOrder(newOrder);

        return Ok("Новый заказ успешно добавлен.");

    }

    [HttpPut("updateorder/{orderId}")]
    public IActionResult UpdateOrder([FromBody] AddOrderModel orderModel, int orderId)
    {
        Order updatedOrder = new Order
        {
            Clientid = orderModel.Idclient,
            Productid = orderModel.Idproduct,
            Date = orderModel.Date,
            Amount = orderModel.Amount,
            Pricebyone = orderModel.Pricebyone,
            Allprice = orderModel.Allprice
        };

        _userService.UpdateOrder(updatedOrder, orderId);

        return Ok("Данные заказа успешно обновлены.");
    }
}