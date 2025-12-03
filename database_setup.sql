-- =====================================================
-- ShopEase E-Commerce Database Setup Script
-- =====================================================
-- This script creates the database schema for ShopEase
-- Run this script if you want to manually set up the database
-- Otherwise, the application will auto-initialize on first run
-- =====================================================

-- Create database
CREATE DATABASE IF NOT EXISTS Shop;
USE Shop;

-- =====================================================
-- Products Table
-- =====================================================
CREATE TABLE IF NOT EXISTS Products (
    ProductID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Category VARCHAR(100) NOT NULL,
    Description TEXT,
    ImageUrl VARCHAR(500),
    StockQuantity INT DEFAULT 0,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_category (Category),
    INDEX idx_price (Price)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- Users Table
-- =====================================================
CREATE TABLE IF NOT EXISTS Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsActive BOOLEAN DEFAULT TRUE,
    INDEX idx_username (Username),
    INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- CartItems Table
-- =====================================================
CREATE TABLE IF NOT EXISTS CartItems (
    CartItemID INT PRIMARY KEY AUTO_INCREMENT,
    UserId VARCHAR(100) NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    AddedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    UNIQUE KEY unique_user_product (UserId, ProductID),
    INDEX idx_userid (UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- Insert Sample Products
-- =====================================================
INSERT INTO Products (Name, Price, Category, Description, ImageUrl, StockQuantity) VALUES
('Laptop', 999.99, 'Electronics', 'High-performance laptop with 16GB RAM and 512GB SSD', 'https://images.unsplash.com/photo-1496181133206-80ce9b88a853', 10),
('Smartphone', 699.99, 'Electronics', 'Latest smartphone with 5G connectivity and advanced camera', 'https://images.unsplash.com/photo-1511707171634-5f897ff02aa9', 15),
('Wireless Headphones', 149.99, 'Electronics', 'Premium noise-canceling wireless headphones', 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e', 20),
('Coffee Maker', 79.99, 'Home & Kitchen', 'Programmable coffee maker with thermal carafe', 'https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6', 12),
('Running Shoes', 89.99, 'Sports', 'Comfortable running shoes with cushioned sole', 'https://images.unsplash.com/photo-1542291026-7eec264c27ff', 25),
('Backpack', 49.99, 'Accessories', 'Durable travel backpack with laptop compartment', 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62', 18),
('Desk Lamp', 34.99, 'Home & Office', 'LED desk lamp with adjustable brightness', 'https://images.unsplash.com/photo-1507473885765-e6ed057f782c', 30),
('Water Bottle', 19.99, 'Sports', 'Insulated stainless steel water bottle', 'https://images.unsplash.com/photo-1602143407151-7111542de6e8', 40),
('Bluetooth Speaker', 59.99, 'Electronics', 'Portable Bluetooth speaker with 12-hour battery', 'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1', 22),
('Yoga Mat', 29.99, 'Sports', 'Non-slip yoga mat with carrying strap', 'https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f', 35),
('Desk Chair', 199.99, 'Home & Office', 'Ergonomic office chair with lumbar support', 'https://images.unsplash.com/photo-1580480055273-228ff5388ef8', 8),
('Tablet', 399.99, 'Electronics', '10-inch tablet with stylus support', 'https://images.unsplash.com/photo-1561154464-82e9adf32764', 14),
('Kitchen Knife Set', 89.99, 'Home & Kitchen', 'Professional chef knife set with block', 'https://images.unsplash.com/photo-1593618998160-e34014e67546', 16),
('Fitness Tracker', 129.99, 'Sports', 'Smart fitness tracker with heart rate monitor', 'https://images.unsplash.com/photo-1575311373937-040b8e1fd5b6', 28),
('Sunglasses', 79.99, 'Accessories', 'Polarized sunglasses with UV protection', 'https://images.unsplash.com/photo-1572635196237-14b3f281503f', 45);

-- =====================================================
-- Verify Installation
-- =====================================================
SELECT 'Database setup completed successfully!' AS Status;
SELECT COUNT(*) AS ProductCount FROM Products;
SELECT 'Sample products inserted' AS Info;

-- =====================================================
-- Useful Queries for Testing
-- =====================================================

-- View all products
-- SELECT * FROM Products;

-- View products by category
-- SELECT * FROM Products WHERE Category = 'Electronics';

-- View all users
-- SELECT UserId, Username, Email, CreatedAt, IsActive FROM Users;

-- View cart items for a user
-- SELECT c.*, p.Name, p.Price 
-- FROM CartItems c 
-- JOIN Products p ON c.ProductID = p.ProductID 
-- WHERE c.UserId = 'your_user_id';

-- Clear all cart items (for testing)
-- DELETE FROM CartItems;

-- Reset auto increment (for testing)
-- ALTER TABLE Products AUTO_INCREMENT = 1;
-- ALTER TABLE Users AUTO_INCREMENT = 1;
-- ALTER TABLE CartItems AUTO_INCREMENT = 1;

-- =====================================================
-- Backup and Restore Commands
-- =====================================================

-- Backup database:
-- mysqldump -u root -p Shop > shop_backup.sql

-- Restore database:
-- mysql -u root -p Shop < shop_backup.sql

-- =====================================================
-- Performance Optimization (Optional)
-- =====================================================

-- Add additional indexes for better query performance
-- CREATE INDEX idx_product_name ON Products(Name);
-- CREATE INDEX idx_cart_added ON CartItems(AddedAt);

-- Analyze tables for optimization
-- ANALYZE TABLE Products;
-- ANALYZE TABLE Users;
-- ANALYZE TABLE CartItems;

-- =====================================================
-- Security Recommendations
-- =====================================================

-- 1. Create a dedicated database user (instead of using root)
-- CREATE USER 'shopease_user'@'localhost' IDENTIFIED BY 'secure_password';
-- GRANT SELECT, INSERT, UPDATE, DELETE ON Shop.* TO 'shopease_user'@'localhost';
-- FLUSH PRIVILEGES;

-- 2. Update connection string to use the new user:
-- Server=localhost;Database=Shop;Uid=shopease_user;Pwd=secure_password;

-- =====================================================
-- Maintenance Queries
-- =====================================================

-- Check database size
-- SELECT 
--     table_schema AS 'Database',
--     ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'Size (MB)'
-- FROM information_schema.tables
-- WHERE table_schema = 'Shop'
-- GROUP BY table_schema;

-- Check table sizes
-- SELECT 
--     table_name AS 'Table',
--     ROUND(((data_length + index_length) / 1024 / 1024), 2) AS 'Size (MB)'
-- FROM information_schema.tables
-- WHERE table_schema = 'Shop'
-- ORDER BY (data_length + index_length) DESC;

-- =====================================================
-- End of Setup Script
-- =====================================================