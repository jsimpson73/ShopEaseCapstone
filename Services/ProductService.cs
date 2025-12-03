using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopEase.Models;

namespace ShopEase.Services
{
    /// <summary>
    /// Service for managing product operations
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IDatabaseService _databaseService;

        public ProductService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Gets all products from the database
        /// </summary>
        public async Task<List<Product>> GetAllProducts()
        {
            return await _databaseService.GetAllProducts();
        }

        /// <summary>
        /// Gets a specific product by ID
        /// </summary>
        public async Task<Product?> GetProductById(int productId)
        {
            return await _databaseService.GetProductById(productId);
        }

        /// <summary>
        /// Gets products filtered by category
        /// </summary>
        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            var allProducts = await _databaseService.GetAllProducts();
            
            if (string.IsNullOrWhiteSpace(category) || category == "All")
            {
                return allProducts;
            }

            return allProducts.Where(p => p.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Gets all unique product categories
        /// </summary>
        public async Task<List<string>> GetCategories()
        {
            var products = await _databaseService.GetAllProducts();
            var categories = products.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
            categories.Insert(0, "All");
            return categories;
        }
    }
}