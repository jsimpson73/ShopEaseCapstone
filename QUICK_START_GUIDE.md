# ShopEase - Quick Start Guide

This guide will help you get the ShopEase e-commerce application up and running in minutes.

## ⚡ Quick Setup (5 Minutes)

### Step 1: Install Prerequisites (2 minutes)

1. **Install .NET 8.0 SDK**
   - Windows: Download from https://dotnet.microsoft.com/download
   - macOS: `brew install dotnet`
   - Linux: Follow instructions at https://docs.microsoft.com/dotnet/core/install/linux

2. **Install MySQL**
   - Windows: Download MySQL Installer from https://dev.mysql.com/downloads/installer/
   - macOS: `brew install mysql`
   - Linux: `sudo apt-get install mysql-server`

### Step 2: Setup Database (1 minute)

1. **Start MySQL**:
   ```bash
   # Windows: MySQL should start automatically
   # macOS: brew services start mysql
   # Linux: sudo systemctl start mysql
   ```

2. **Create Database**:
   ```bash
   mysql -u root -p
   ```
   Then run:
   ```sql
   CREATE DATABASE Shop;
   exit;
   ```

3. **Update Connection String**:
   - Open `Services/DatabaseService.cs`
   - Find line 18: `_connectionString = "Server=localhost;Database=Shop;Uid=root;Pwd=password;";`
   - Replace `password` with your MySQL root password

### Step 3: Run Application (2 minutes)

1. **Navigate to project folder**:
   ```bash
   cd ShopEase
   ```

2. **Restore packages**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Open browser**:
   - Go to: `https://localhost:5001` or `http://localhost:5000`
   - The database will initialize automatically!

## 🎯 First Steps

### 1. Register an Account
- Click **"Register"** in the top navigation
- Fill in:
  - Username: `testuser`
  - Email: `test@example.com`
  - Password: `password123`
- Click **"Register"**

### 2. Login
- Click **"Login"** in the top navigation
- Enter your credentials
- Click **"Login"**

### 3. Browse Products
- You'll see 15 sample products on the home page
- Use the category filter to browse by category
- Click **"Add to Cart"** on any product

### 4. View Cart
- Click **"Cart"** in the navigation (you'll see a badge with item count)
- Adjust quantities or remove items
- See your order total with tax calculation

## 🔧 Troubleshooting

### Problem: "Cannot connect to MySQL"
**Solution**:
```bash
# Check if MySQL is running
mysql --version

# Start MySQL
# macOS: brew services start mysql
# Linux: sudo systemctl start mysql
# Windows: Check Services app
```

### Problem: "Database 'Shop' does not exist"
**Solution**:
```bash
mysql -u root -p
CREATE DATABASE Shop;
exit;
```

### Problem: "Build failed"
**Solution**:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Problem: "Port already in use"
**Solution**:
- The app uses ports 5000 (HTTP) and 5001 (HTTPS)
- Stop any other applications using these ports
- Or change ports in `Properties/launchSettings.json`

## 📱 Testing the Application

### Test Product Browsing
1. ✅ View all products on home page
2. ✅ Filter by category (Electronics, Sports, etc.)
3. ✅ Check product details display correctly

### Test Shopping Cart
1. ✅ Add multiple products to cart
2. ✅ View cart page
3. ✅ Update quantities
4. ✅ Remove items
5. ✅ Refresh page - cart should persist!

### Test Authentication
1. ✅ Register new account
2. ✅ Login with credentials
3. ✅ Logout
4. ✅ Try accessing cart without login (should redirect)

### Test Responsiveness
1. ✅ Resize browser window
2. ✅ Test on mobile device
3. ✅ Check tablet view
4. ✅ Verify all features work on all screen sizes

## 🎨 Customization

### Change Sample Products
Edit `Services/DatabaseService.cs`, method `InsertSampleProducts()` (around line 120)

### Change Color Scheme
Edit `wwwroot/css/site.css`, CSS variables at the top (around line 10)

### Change Database Connection
Edit `Services/DatabaseService.cs`, line 18

## 📊 Database Management

### View Database Tables
```bash
mysql -u root -p Shop
SHOW TABLES;
SELECT * FROM Products;
SELECT * FROM Users;
SELECT * FROM CartItems;
```

### Reset Database
```bash
mysql -u root -p Shop
DROP DATABASE Shop;
CREATE DATABASE Shop;
```
Then restart the application to reinitialize.

### Backup Database
```bash
mysqldump -u root -p Shop > shop_backup.sql
```

### Restore Database
```bash
mysql -u root -p Shop < shop_backup.sql
```

## 🚀 Next Steps

1. **Read Full Documentation**: See `PROJECT_DOCUMENTATION.md` for detailed information
2. **Run Tests**: Execute `TestProgram.cs` to see business logic in action
3. **Customize**: Modify products, colors, and features
4. **Deploy**: Follow deployment guide in README.md

## 💡 Tips

- **Cart Persistence**: Your cart is saved in both the database and browser local storage
- **Security**: All passwords are hashed, and SQL injection is prevented
- **Responsive**: The app works great on mobile, tablet, and desktop
- **Accessibility**: Fully keyboard navigable with screen reader support

## 📞 Need Help?

- Check `README.md` for detailed setup instructions
- Read `PROJECT_DOCUMENTATION.md` for comprehensive documentation
- Review `database_setup.sql` for manual database setup

## ✅ Success Checklist

- [ ] .NET 8.0 SDK installed
- [ ] MySQL installed and running
- [ ] Database created
- [ ] Connection string updated
- [ ] Application running
- [ ] Can access http://localhost:5000
- [ ] Can register and login
- [ ] Can add items to cart
- [ ] Cart persists after refresh

**Congratulations! You're ready to use ShopEase! 🎉**

---

**Estimated Setup Time**: 5 minutes  
**Difficulty**: Easy  
**Prerequisites**: Basic command line knowledge