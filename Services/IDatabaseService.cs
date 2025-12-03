using System.Collections.Generic;
using System.Threading.Tasks;
using ShopEase.Models;

namespace ShopEase.Services
{
    /// <summary>
    /// Interface for database operations
    /// </summary>
    public interface IDatabaseService
    {
        Task InitializeDatabase();
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<bool> AddCartItem(string userId, int productId, int quantity);
        Task<bool> UpdateCartItem(string userId, int productId, int quantity);
        Task<bool> RemoveCartItem(string userId, int productId);
        Task<List<CartItem>> GetCartItems(string userId);
        Task<bool> ClearCart(string userId);
        Task<User?> GetUserByUsername(string username);
        Task<bool> CreateUser(User user);
        Task<bool> ValidateUser(string username, string password);
    }
}