using Backend;
using Backend.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IUserService _userService;


    public ProductController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("products")]
    public List<ProductModel> GetAllProducts()
    {
        List<ProductModel> productModels = new List<ProductModel>();
        _userService.GetAllProduct().ForEach(product =>
        {
            productModels.Add(new ProductModel(product));
        });
        return productModels;
    }

    [HttpDelete("delproduct/{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        _userService.DeleteProduct(productId);
        return Ok("Пользователь успешно удален.");
    }

    [HttpPost("addproduct")]
    public IActionResult AddProduct([FromBody] AddProductModel productModel)
    {

        Product newProduct = new Product
        {
            Title = productModel.Title,
            Category = productModel.Category,
            Brand = productModel.Brand,
            Price = productModel.Price,
            Amount = productModel.Amount,
            Minamount = productModel.Minamount,
            Providerid = productModel.Idprovider,
        };

        _userService.AddProduct(newProduct);

        return Ok("Новый продукт успешно добавлен.");

    }
    [HttpPut("updateproduct/{productId}")]
    public IActionResult UpdateProduct([FromBody] AddProductModel productModel, int productId)
    {
        Product updatedProduct = new Product
        {
            Title = productModel.Title,
            Category = productModel.Category,
            Brand = productModel.Brand,
            Price = productModel.Price,
            Amount = productModel.Amount,
            Minamount = productModel.Minamount,
            Providerid = productModel.Idprovider,
        };

        _userService.UpdateProduct(updatedProduct, productId);

        return Ok("Данные продукта успешно обновлены.");
    }
}