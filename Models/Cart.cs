using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopEase.Services;

namespace ShopEase.Models
{
    /// <summary>
    /// Represents a shopping cart with database persistence
    /// </summary>
    public class Cart
    {
        private List<CartItem> _items;
        private readonly IDatabaseService _databaseService;
        public string UserId { get; set; } = string.Empty;

        public Cart(IDatabaseService databaseService, string userId = "guest")
        {
            _items = new List<CartItem>();
            _databaseService = databaseService;
            UserId = userId;
        }

        /// <summary>
        /// Gets all items in the cart
        /// </summary>
        public List<CartItem> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// Adds a product to the cart and saves to database
        /// </summary>
        public async Task<bool> AddProduct(Product product)
        {
            if (product == null || !product.IsValid())
            {
                Console.WriteLine("Invalid product. Cannot add to cart.");
                return false;
            }

            // Check if product already exists in cart
            var existingItem = _items.FirstOrDefault(item => item.Product.ProductID == product.ProductID);
            
            if (existingItem != null)
            {
                existingItem.Quantity++;
                await _databaseService.UpdateCartItem(UserId, product.ProductID, existingItem.Quantity);
            }
            else
            {
                var cartItem = new CartItem
                {
                    Product = product,
                    Quantity = 1
                };
                _items.Add(cartItem);
                await _databaseService.AddCartItem(UserId, product.ProductID, 1);
            }

            Console.WriteLine($"Added {product.Name} to cart.");
            return true;
        }

        /// <summary>
        /// Removes a product from the cart by ID and updates database
        /// </summary>
        public async Task<bool> RemoveProduct(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.ProductID == productId);
            
            if (item != null)
            {
                _items.Remove(item);
                await _databaseService.RemoveCartItem(UserId, productId);
                Console.WriteLine($"Removed product ID {productId} from cart.");
                return true;
            }
            
            Console.WriteLine($"Product ID {productId} not found in cart.");
            return false;
        }

        /// <summary>
        /// Updates the quantity of a product in the cart
        /// </summary>
        public async Task<bool> UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveProduct(productId);
            }

            var item = _items.FirstOrDefault(i => i.Product.ProductID == productId);
            
            if (item != null)
            {
                item.Quantity = quantity;
                await _databaseService.UpdateCartItem(UserId, productId, quantity);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Displays all items in the cart
        /// </summary>
        public void DisplayCartItems()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            Console.WriteLine("\n=== Shopping Cart ===");
            foreach (var item in _items)
            {
                Console.WriteLine($"{item.Product.Name} - Quantity: {item.Quantity} - Subtotal: ${item.GetSubtotal():F2}");
            }
            Console.WriteLine($"Total: ${CalculateTotal():F2}");
            Console.WriteLine("====================\n");
        }

        /// <summary>
        /// Calculates the total price of all items in the cart
        /// </summary>
        public decimal CalculateTotal()
        {
            return _items.Sum(item => item.GetSubtotal());
        }

        /// <summary>
        /// Gets the total number of items in the cart
        /// </summary>
        public int GetItemCount()
        {
            return _items.Sum(item => item.Quantity);
        }

        /// <summary>
        /// Clears all items from the cart
        /// </summary>
        public async Task ClearCart()
        {
            _items.Clear();
            await _databaseService.ClearCart(UserId);
            Console.WriteLine("Cart cleared.");
        }

        /// <summary>
        /// Loads cart items from database
        /// </summary>
        public async Task LoadFromDatabase()
        {
            _items = await _databaseService.GetCartItems(UserId);
        }
    }

    /// <summary>
    /// Represents an item in the shopping cart
    /// </summary>
    public class CartItem
    {
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }

        public decimal GetSubtotal()
        {
            return Product.Price * Quantity;
        }
    }
}