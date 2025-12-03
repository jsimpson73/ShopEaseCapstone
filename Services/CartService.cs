using System;
using System.Threading.Tasks;
using ShopEase.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ShopEase.Services
{
    /// <summary>
    /// Service for managing shopping cart with state persistence
    /// </summary>
    public class CartService : ICartService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private Cart? _cart;
        private string _currentUserId = "guest";

        public event Action? OnCartChanged;

        public CartService(
            IDatabaseService databaseService, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _databaseService = databaseService;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        /// <summary>
        /// Gets the current user's cart
        /// </summary>
        public async Task<Cart> GetCart()
        {
            if (_cart == null)
            {
                await InitializeCart();
            }
            return _cart!;
        }

        /// <summary>
        /// Initializes the cart for the current user
        /// </summary>
        private async Task InitializeCart()
        {
            // Get current user
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            
            if (user.Identity?.IsAuthenticated == true)
            {
                _currentUserId = user.Identity.Name ?? "guest";
            }
            else
            {
                _currentUserId = "guest";
            }

            _cart = new Cart(_databaseService, _currentUserId);
            await _cart.LoadFromDatabase();

            // Also load from local storage for persistence
            await LoadFromLocalStorage();
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        public async Task AddToCart(Product product)
        {
            var cart = await GetCart();
            await cart.AddProduct(product);
            await SaveToLocalStorage();
            OnCartChanged?.Invoke();
        }

        /// <summary>
        /// Removes a product from the cart
        /// </summary>
        public async Task RemoveFromCart(int productId)
        {
            var cart = await GetCart();
            await cart.RemoveProduct(productId);
            await SaveToLocalStorage();
            OnCartChanged?.Invoke();
        }

        /// <summary>
        /// Updates the quantity of a product in the cart
        /// </summary>
        public async Task UpdateQuantity(int productId, int quantity)
        {
            var cart = await GetCart();
            await cart.UpdateQuantity(productId, quantity);
            await SaveToLocalStorage();
            OnCartChanged?.Invoke();
        }

        /// <summary>
        /// Clears all items from the cart
        /// </summary>
        public async Task ClearCart()
        {
            var cart = await GetCart();
            await cart.ClearCart();
            await SaveToLocalStorage();
            OnCartChanged?.Invoke();
        }

        /// <summary>
        /// Gets the total number of items in the cart
        /// </summary>
        public async Task<int> GetCartItemCount()
        {
            var cart = await GetCart();
            return cart.GetItemCount();
        }

        /// <summary>
        /// Gets the total price of all items in the cart
        /// </summary>
        public async Task<decimal> GetCartTotal()
        {
            var cart = await GetCart();
            return cart.CalculateTotal();
        }

        /// <summary>
        /// Saves cart state to local storage for persistence
        /// </summary>
        private async Task SaveToLocalStorage()
        {
            if (_cart != null)
            {
                var items = _cart.GetItems();
                await _localStorage.SetItemAsync($"cart_{_currentUserId}", items);
            }
        }

        /// <summary>
        /// Loads cart state from local storage
        /// </summary>
        private async Task LoadFromLocalStorage()
        {
            try
            {
                var items = await _localStorage.GetItemAsync<System.Collections.Generic.List<CartItem>>($"cart_{_currentUserId}");
                if (items != null && _cart != null)
                {
                    foreach (var item in items)
                    {
                        // Verify item exists in database before adding
                        var product = await _databaseService.GetProductById(item.Product.ProductID);
                        if (product != null)
                        {
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                await _cart.AddProduct(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from local storage: {ex.Message}");
            }
        }
    }
}