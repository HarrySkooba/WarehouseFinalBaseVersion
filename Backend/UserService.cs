using Backend;
using Backend.DB;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly PostgresContext _context;
    public UserService(PostgresContext context)
    {
        _context = context;
    }

    public User GetUserByLogin(string username, string password)
    {
        return _context.Users.FirstOrDefault(u => u.Name == username && u.Password == password);
    }
    public Role GetRoleByLogin(int idrole)
    {
        return _context.Roles.FirstOrDefault(r => r.Id == idrole);
    }

    public List<User> GetAllUser()
    {
        return _context.Users.Include(u => u.Role).ToList();
    }
    public List<Role> GetAllRole()
    {
        return _context.Roles.ToList();
    }
    public List<Product> GetAllProduct()
    {
        return _context.Products.Include(u => u.Provider).ToList();
    }
    public List<Provider> GetAllProvider()
    {
        return _context.Providers.ToList();
    }
    public List<Client> GetAllClient()
    {
        return _context.Clients.ToList();
    }
    public List<Supply> GetAllSupply()
    {
        return _context.Supplies.Include(u => u.Product).Include(u => u.Provider).ToList();
    }
    public List<Order> GetAllOrder()
    {
        return _context.Orders.Include(u => u.Product).Include(u => u.Client).ToList();
    }

    public void DeleteUser(int userId)
    {
        var userToDelete = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (userToDelete != null)
        {
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            Console.WriteLine("Пользователь успешно удален.");
        }
        else
        {
            Console.WriteLine("Пользователь с указанным Id не найден.");
        }
    }
    public void DeleteRole(int roleId)
    {
        var roleToDelete = _context.Roles.FirstOrDefault(u => u.Id == roleId);

        if (roleToDelete != null)
        {
            _context.Roles.Remove(roleToDelete);
            _context.SaveChanges();
            Console.WriteLine("Роль успешно удалена.");
        }
        else
        {
            Console.WriteLine("Роль с указанным Id не найден.");
        }
    }
    public void DeleteProduct(int productId)
    {
        var productToDelete = _context.Products.FirstOrDefault(u => u.Id == productId);

        if (productToDelete != null)
        {
            _context.Products.Remove(productToDelete);
            _context.SaveChanges();
            Console.WriteLine("Продукт успешно удален.");
        }
        else
        {
            Console.WriteLine("Продукт с указанным Id не найден.");
        }
    }
    public void DeleteProvider(int providerId)
    {
        var providerToDelete = _context.Providers.FirstOrDefault(u => u.Id == providerId);

        if (providerToDelete != null)
        {
            _context.Providers.Remove(providerToDelete);
            _context.SaveChanges();
            Console.WriteLine("Поставщик успешно удален.");
        }
        else
        {
            Console.WriteLine("Поставщик с указанным Id не найден.");
        }
    }
    public void DeleteClient(int clientId)
    {
        var clientToDelete = _context.Clients.FirstOrDefault(u => u.Id == clientId);

        if (clientToDelete != null)
        {
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
            Console.WriteLine("Заказчик успешно удален.");
        }
        else
        {
            Console.WriteLine("Заказчик с указанным Id не найден.");
        }
    }
    public void DeleteSupply(int supplyId)
    {
        var supplyToDelete = _context.Supplies.FirstOrDefault(u => u.Id == supplyId);

        if (supplyToDelete != null)
        {
            _context.Supplies.Remove(supplyToDelete);
            _context.SaveChanges();
            Console.WriteLine("Поставка успешно удалена.");
        }
        else
        {
            Console.WriteLine("Поставка с указанным Id не найден.");
        }
    }
    public void DeleteOrder(int orderId)
    {
        var orderToDelete = _context.Orders.FirstOrDefault(u => u.Id == orderId);

        if (orderToDelete != null)
        {
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();
            Console.WriteLine("Заказ успешно удален.");
        }
        else
        {
            Console.WriteLine("Заказ с указанным Id не найден.");
        }
    }

    public async Task AddUser(User newUser)
    {
        using (var context = new PostgresContext()) 
        {
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddRole(Role newRole)
    {
        using (var context = new PostgresContext())
        {
            context.Roles.Add(newRole);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddProduct(Product newProduct)
    {
        using (var context = new PostgresContext())
        {
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddProvider(Provider newProvider)
    {
        using (var context = new PostgresContext())
        {
            context.Providers.Add(newProvider);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddClient(Client newClient)
    {
        using (var context = new PostgresContext())
        {
            context.Clients.Add(newClient);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddSupply(Supply newSupply)
    {
        using (var context = new PostgresContext())
        {
            context.Supplies.Add(newSupply);
            await context.SaveChangesAsync();
        }
    }
    public async Task AddOrder(Order newOrder)
    {
        using (var context = new PostgresContext())
        {
            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateUser(User updatedUser, int userId)
    {
        using (var context = new PostgresContext())
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Id == userId);

            if (existingUser != null)
            {
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.Password = updatedUser.Password;
                existingUser.Roleid = updatedUser.Roleid;
                existingUser.Adminpanel = updatedUser.Adminpanel;

                context.Users.Update(existingUser);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateRole(Role updatedRole, int roleId)
    {
        using (var context = new PostgresContext())
        {
            var existingRole = context.Roles.FirstOrDefault(u => u.Id == roleId);

            if (existingRole != null)
            {
                existingRole.Role1 = updatedRole.Role1;

                context.Roles.Update(existingRole);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateProduct(Product updatedProduct, int productId)
    {
        using (var context = new PostgresContext())
        {
            var existingProduct = context.Products.FirstOrDefault(u => u.Id == productId);

            if (existingProduct != null)
            {
                existingProduct.Title = updatedProduct.Title;
                existingProduct.Category = updatedProduct.Category;
                existingProduct.Brand = updatedProduct.Brand;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Amount = updatedProduct.Amount;
                existingProduct.Minamount = updatedProduct.Minamount;
                existingProduct.Providerid = updatedProduct.Providerid;

                context.Products.Update(existingProduct);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateProvider(Provider updatedProvider, int providerId)
    {
        using (var context = new PostgresContext())
        {
            var existingProvider = context.Providers.FirstOrDefault(u => u.Id == providerId);

            if (existingProvider != null)
            {
                existingProvider.Title = updatedProvider.Title;
                existingProvider.Info = updatedProvider.Info;
                existingProvider.Number = updatedProvider.Number;
                existingProvider.Email = updatedProvider.Email;

                context.Providers.Update(existingProvider);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateClient(Client updatedClient, int clientId)
    {
        using (var context = new PostgresContext())
        {
            var existingClient = context.Clients.FirstOrDefault(u => u.Id == clientId);

            if (existingClient != null)
            {
                existingClient.Title = updatedClient.Title;
                existingClient.Info = updatedClient.Info;
                existingClient.Number = updatedClient.Number;
                existingClient.Email = updatedClient.Email;

                context.Clients.Update(existingClient);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateSupply(Supply updatedSupply, int supplyId)
    {
        using (var context = new PostgresContext())
        {
            var existingSupply = context.Supplies.FirstOrDefault(u => u.Id == supplyId);

            if (existingSupply != null)
            {
                existingSupply.Providerid = updatedSupply.Providerid;
                existingSupply.Productid = updatedSupply.Productid;
                existingSupply.Date = updatedSupply.Date;
                existingSupply.Amount = updatedSupply.Amount;
                existingSupply.Pricebyone = updatedSupply.Pricebyone;
                existingSupply.Allprice = updatedSupply.Allprice;

                context.Supplies.Update(existingSupply);
                await context.SaveChangesAsync();
            }
        }
    }
    public async Task UpdateOrder(Order updatedOrder, int orderId)
    {
        using (var context = new PostgresContext())
        {
            var existingOrder = context.Orders.FirstOrDefault(u => u.Id == orderId);

            if (existingOrder != null)
            {
                existingOrder.Clientid = updatedOrder.Clientid;
                existingOrder.Productid = updatedOrder.Productid;
                existingOrder.Date = updatedOrder.Date;
                existingOrder.Amount = updatedOrder.Amount;
                existingOrder.Pricebyone = updatedOrder.Pricebyone;
                existingOrder.Allprice = updatedOrder.Allprice;

                context.Orders.Update(existingOrder);
                await context.SaveChangesAsync();
            }
        }
    }


}