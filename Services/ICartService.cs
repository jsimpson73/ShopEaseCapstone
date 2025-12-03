using System.Threading.Tasks;
using ShopEase.Models;

namespace ShopEase.Services
{
    /// <summary>
    /// Interface for cart service operations
    /// </summary>
    public interface ICartService
    {
        Task<Cart> GetCart();
        Task AddToCart(Product product);
        Task RemoveFromCart(int productId);
        Task UpdateQuantity(int productId, int quantity);
        Task ClearCart();
        Task<int> GetCartItemCount();
        Task<decimal> GetCartTotal();
    }
}