using System.Collections.Generic;
using System.Threading.Tasks;
using ShopEase.Models;

namespace ShopEase.Services
{
    /// <summary>
    /// Interface for product service operations
    /// </summary>
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<Product>> GetProductsByCategory(string category);
        Task<List<string>> GetCategories();
    }
}