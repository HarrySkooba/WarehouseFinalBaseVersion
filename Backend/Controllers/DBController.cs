using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DB;
using Backend;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        private readonly PostgresContext _context;

        public DBController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            var usersWithRoleNames = users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Password,
                RoleName = u.Role.Role1
            }).ToList();

            return usersWithRoleNames;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            var Roles = roles.Select(u => new
            {
                u.Id,
                u.Role1
        }).ToList();

            return Roles;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts()
        {
            var products = await _context.Products.Include(u => u.Provider).ToListAsync();
            var productsProviderNames = products.Select(u => new
            {
                u.Id,
                u.Title,
                u.Category,
                u.Price,
                u.Amount,
                u.Minamount,
                ProviderName = u.Provider.Title
            }).ToList();

            return productsProviderNames;
        }

        [HttpGet("provider")]
        public async Task<ActionResult<IEnumerable<object>>> GetProvidersWithProducts()
        {
            var providers = await _context.Providers.Include(p => p.Products).ToListAsync();
            var providersWithProducts = providers.Select(p => new
            {
                Provider = new
                {
                    p.Id,
                    p.Title,
                    p.Info,
                    p.Number,
                    p.Email
                },
                Products = p.Products.Select(pr => new
                {
                    pr.Title,
                    pr.Brand
                }).ToList()
            }).ToList();

            return providersWithProducts;
        }

        [HttpGet("supply")]
        public async Task<ActionResult<IEnumerable<object>>> GetSupplies()
        {
            var supplies = await _context.Supplies
                .Include(s => s.Product)
                .Include(s => s.Provider)
                .ToListAsync();

            var suppliesWithNames = supplies.Select(s => new
            {
                s.Id,
                ProductName = s.Product.Title,
                ProviderName = s.Provider.Title,
                s.Amount,
                s.Date,
                s.Pricebyone
            }).ToList();

            return suppliesWithNames;


        }
     
    }
    
}