using System;

namespace ShopEase.Models
{
    /// <summary>
    /// Represents a product in the e-commerce system
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int StockQuantity { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Product()
        {
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public Product(int productId, string name, decimal price, string category, string description = "", string imageUrl = "", int stockQuantity = 0)
        {
            ProductID = productId;
            Name = SanitizeInput(name);
            Price = price;
            Category = SanitizeInput(category);
            Description = SanitizeInput(description);
            ImageUrl = SanitizeInput(imageUrl);
            StockQuantity = stockQuantity;
        }

        /// <summary>
        /// Prints product details in a formatted manner
        /// </summary>
        public void PrintDetails()
        {
            Console.WriteLine($"Product: {Name} | Price: ${Price:F2} | Category: {Category}");
        }

        /// <summary>
        /// Returns formatted product details as a string
        /// </summary>
        public string GetFormattedDetails()
        {
            return $"Product: {Name} | Price: ${Price:F2} | Category: {Category}";
        }

        /// <summary>
        /// Sanitizes input to prevent XSS attacks
        /// </summary>
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove potentially dangerous characters
            return System.Net.WebUtility.HtmlEncode(input.Trim());
        }

        /// <summary>
        /// Validates product data
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   Price > 0 && 
                   !string.IsNullOrWhiteSpace(Category);
        }

        public override string ToString()
        {
            return GetFormattedDetails();
        }
    }
}