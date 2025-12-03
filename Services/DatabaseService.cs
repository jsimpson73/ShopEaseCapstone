using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ShopEase.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShopEase.Services
{
    /// <summary>
    /// Handles all database operations with MySQL
    /// Implements security measures against SQL injection
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            // Connection string - In production, this should be in configuration
            _connectionString = "Server=localhost;Database=Shop;Uid=root;Pwd=password;";
        }

        /// <summary>
        /// Initializes the database and creates necessary tables
        /// </summary>
        public async Task InitializeDatabase()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Create Products table
                string createProductsTable = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        ProductID INT PRIMARY KEY AUTO_INCREMENT,
                        Name VARCHAR(255) NOT NULL,
                        Price DECIMAL(10, 2) NOT NULL,
                        Category VARCHAR(100) NOT NULL,
                        Description TEXT,
                        ImageUrl VARCHAR(500),
                        StockQuantity INT DEFAULT 0,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";

                using (var cmd = new MySqlCommand(createProductsTable, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                // Create CartItems table
                string createCartItemsTable = @"
                    CREATE TABLE IF NOT EXISTS CartItems (
                        CartItemID INT PRIMARY KEY AUTO_INCREMENT,
                        UserId VARCHAR(100) NOT NULL,
                        ProductID INT NOT NULL,
                        Quantity INT NOT NULL,
                        AddedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
                        UNIQUE KEY unique_user_product (UserId, ProductID)
                    )";

                using (var cmd = new MySqlCommand(createCartItemsTable, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                // Create Users table
                string createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        UserId INT PRIMARY KEY AUTO_INCREMENT,
                        Username VARCHAR(50) UNIQUE NOT NULL,
                        Email VARCHAR(100) UNIQUE NOT NULL,
                        PasswordHash VARCHAR(255) NOT NULL,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        IsActive BOOLEAN DEFAULT TRUE
                    )";

                using (var cmd = new MySqlCommand(createUsersTable, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                // Insert sample products if table is empty
                await InsertSampleProducts(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Inserts sample products for testing
        /// </summary>
        private async Task InsertSampleProducts(MySqlConnection connection)
        {
            string checkQuery = "SELECT COUNT(*) FROM Products";
            using var checkCmd = new MySqlCommand(checkQuery, connection);
            var count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

            if (count == 0)
            {
                var sampleProducts = new[]
                {
                    ("Laptop", 999.99m, "Electronics", "High-performance laptop", "https://images.unsplash.com/photo-1496181133206-80ce9b88a853", 10),
                    ("Smartphone", 699.99m, "Electronics", "Latest smartphone model", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9", 15),
                    ("Headphones", 149.99m, "Electronics", "Wireless noise-canceling headphones", "https://images.unsplash.com/photo-1505740420928-5e560c06d30e", 20),
                    ("Coffee Maker", 79.99m, "Home & Kitchen", "Programmable coffee maker", "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6", 12),
                    ("Running Shoes", 89.99m, "Sports", "Comfortable running shoes", "https://images.unsplash.com/photo-1542291026-7eec264c27ff", 25),
                    ("Backpack", 49.99m, "Accessories", "Durable travel backpack", "https://images.unsplash.com/photo-1553062407-98eeb64c6a62", 18),
                    ("Desk Lamp", 34.99m, "Home & Office", "LED desk lamp", "https://images.unsplash.com/photo-1507473885765-e6ed057f782c", 30),
                    ("Water Bottle", 19.99m, "Sports", "Insulated water bottle", "https://images.unsplash.com/photo-1602143407151-7111542de6e8", 40)
                };

                string insertQuery = @"
                    INSERT INTO Products (Name, Price, Category, Description, ImageUrl, StockQuantity) 
                    VALUES (@Name, @Price, @Category, @Description, @ImageUrl, @StockQuantity)";

                foreach (var product in sampleProducts)
                {
                    using var cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Name", product.Item1);
                    cmd.Parameters.AddWithValue("@Price", product.Item2);
                    cmd.Parameters.AddWithValue("@Category", product.Item3);
                    cmd.Parameters.AddWithValue("@Description", product.Item4);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.Item5);
                    cmd.Parameters.AddWithValue("@StockQuantity", product.Item6);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Gets all products from the database
        /// Uses parameterized queries to prevent SQL injection
        /// </summary>
        public async Task<List<Product>> GetAllProducts()
        {
            var products = new List<Product>();

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT ProductID, Name, Price, Category, Description, ImageUrl, StockQuantity FROM Products";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        Name = reader.GetString("Name"),
                        Price = reader.GetDecimal("Price"),
                        Category = reader.GetString("Category"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl"),
                        StockQuantity = reader.GetInt32("StockQuantity")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
            }

            return products;
        }

        /// <summary>
        /// Gets a specific product by ID
        /// </summary>
        public async Task<Product?> GetProductById(int productId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT ProductID, Name, Price, Category, Description, ImageUrl, StockQuantity FROM Products WHERE ProductID = @ProductID";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        Name = reader.GetString("Name"),
                        Price = reader.GetDecimal("Price"),
                        Category = reader.GetString("Category"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl"),
                        StockQuantity = reader.GetInt32("StockQuantity")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Adds an item to the cart in the database
        /// Uses parameterized queries to prevent SQL injection
        /// </summary>
        public async Task<bool> AddCartItem(string userId, int productId, int quantity)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO CartItems (UserId, ProductID, Quantity) 
                    VALUES (@UserId, @ProductID, @Quantity)
                    ON DUPLICATE KEY UPDATE Quantity = Quantity + @Quantity";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cart item: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates the quantity of a cart item
        /// </summary>
        public async Task<bool> UpdateCartItem(string userId, int productId, int quantity)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = @"
                    UPDATE CartItems 
                    SET Quantity = @Quantity 
                    WHERE UserId = @UserId AND ProductID = @ProductID";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart item: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Removes an item from the cart
        /// </summary>
        public async Task<bool> RemoveCartItem(string userId, int productId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "DELETE FROM CartItems WHERE UserId = @UserId AND ProductID = @ProductID";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing cart item: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets all cart items for a user
        /// </summary>
        public async Task<List<CartItem>> GetCartItems(string userId)
        {
            var cartItems = new List<CartItem>();

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = @"
                    SELECT c.Quantity, p.ProductID, p.Name, p.Price, p.Category, p.Description, p.ImageUrl, p.StockQuantity
                    FROM CartItems c
                    INNER JOIN Products p ON c.ProductID = p.ProductID
                    WHERE c.UserId = @UserId";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    cartItems.Add(new CartItem
                    {
                        Quantity = reader.GetInt32("Quantity"),
                        Product = new Product
                        {
                            ProductID = reader.GetInt32("ProductID"),
                            Name = reader.GetString("Name"),
                            Price = reader.GetDecimal("Price"),
                            Category = reader.GetString("Category"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl"),
                            StockQuantity = reader.GetInt32("StockQuantity")
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cart items: {ex.Message}");
            }

            return cartItems;
        }

        /// <summary>
        /// Clears all items from a user's cart
        /// </summary>
        public async Task<bool> ClearCart(string userId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "DELETE FROM CartItems WHERE UserId = @UserId";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets a user by username
        /// </summary>
        public async Task<User?> GetUserByUsername(string username)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT UserId, Username, Email, PasswordHash, CreatedAt, IsActive FROM Users WHERE Username = @Username";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        UserId = reader.GetInt32("UserId"),
                        Username = reader.GetString("Username"),
                        Email = reader.GetString("Email"),
                        PasswordHash = reader.GetString("PasswordHash"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        IsActive = reader.GetBoolean("IsActive")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO Users (Username, Email, PasswordHash, IsActive) 
                    VALUES (@Username, @Email, @PasswordHash, @IsActive)";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates user credentials
        /// </summary>
        public async Task<bool> ValidateUser(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if (user == null || !user.IsActive)
                return false;

            string hashedPassword = HashPassword(password);
            return user.PasswordHash == hashedPassword;
        }

        /// <summary>
        /// Hashes a password using SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}