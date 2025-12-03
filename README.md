# ShopEase E-Commerce Application

A full-featured e-commerce web application built with **Blazor WebAssembly** and **C#**, featuring MySQL database integration, user authentication, shopping cart management, and responsive design.

## 🚀 Features

- **Product Catalog**: Browse products with category filtering
- **Shopping Cart**: Add, remove, and update product quantities
- **User Authentication**: Secure registration and login system
- **Database Integration**: MySQL database with automatic initialization
- **State Persistence**: Cart data persists across page refreshes
- **Responsive Design**: Mobile-first design that works on all devices
- **Security**: SQL injection prevention, XSS protection, password hashing
- **Accessibility**: WCAG 2.1 AA compliant with keyboard navigation

## 📋 Prerequisites

Before running the application, ensure you have the following installed:

1. **.NET 8.0 SDK** or later
   - Download: https://dotnet.microsoft.com/download

2. **MySQL Server** (8.0 or later)
   - Download: https://dev.mysql.com/downloads/mysql/

3. **IDE** (Optional but recommended)
   - Visual Studio 2022
   - Visual Studio Code with C# extension
   - JetBrains Rider

## 🔧 Installation & Setup

### Step 1: Database Setup

1. **Install and start MySQL Server**

2. **Create the database**:
   ```sql
   CREATE DATABASE Shop;
   ```

3. **Update connection string** in `Services/DatabaseService.cs`:
   ```csharp
   _connectionString = "Server=localhost;Database=Shop;Uid=root;Pwd=YOUR_PASSWORD;";
   ```
   Replace `YOUR_PASSWORD` with your MySQL root password.

### Step 2: Application Setup

1. **Navigate to project directory**:
   ```bash
   cd ShopEase
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Access the application**:
   - Open your browser to: `https://localhost:5001` or `http://localhost:5000`
   - The database will be automatically initialized on first run

### Step 3: First-Time Usage

1. **Register a new account**:
   - Click "Register" in the navigation menu
   - Fill in username, email, and password
   - Submit the form

2. **Login**:
   - Click "Login" in the navigation menu
   - Enter your credentials
   - You'll be redirected to the home page

3. **Browse and shop**:
   - View products on the home page
   - Filter by category
   - Add items to your cart
   - View and manage your cart

## 🧪 Running Tests

To run the test program that demonstrates the business logic:

```bash
dotnet run --project TestProgram.cs
```

This will test:
- Product class functionality
- Cart class operations
- Database integration
- Input validation
- Security measures

## 📁 Project Structure

```
ShopEase/
├── Models/
│   ├── Product.cs          # Product data model
│   ├── Cart.cs             # Shopping cart logic
│   └── User.cs             # User data model
├── Services/
│   ├── IDatabaseService.cs      # Database interface
│   ├── DatabaseService.cs       # MySQL implementation
│   ├── IProductService.cs       # Product service interface
│   ├── ProductService.cs        # Product operations
│   ├── ICartService.cs          # Cart service interface
│   ├── CartService.cs           # Cart management
│   └── CustomAuthStateProvider.cs  # Authentication
├── Pages/
│   ├── Index.razor         # Home page with product listing
│   ├── Cart.razor          # Shopping cart page
│   ├── ProductCard.razor   # Product display component
│   ├── Login.razor         # Login page
│   └── Register.razor      # Registration page
├── Shared/
│   └── MainLayout.razor    # Application layout
├── wwwroot/
│   ├── css/
│   │   └── site.css        # Custom styles
│   └── index.html          # HTML entry point
├── Program.cs              # Application entry point
├── App.razor               # Root component
├── _Imports.razor          # Global imports
├── ShopEase.csproj         # Project file
├── TestProgram.cs          # Test program
├── README.md               # This file
└── PROJECT_DOCUMENTATION.md # Comprehensive documentation
```

## 🔒 Security Features

- **SQL Injection Prevention**: All queries use parameterized statements
- **XSS Protection**: Input sanitization with HTML encoding
- **Password Security**: SHA256 hashing for password storage
- **Authentication**: Session-based authentication with secure tokens
- **Input Validation**: Multi-layer validation (client, service, database)
- **Authorization**: Protected routes requiring authentication

## 🎨 Technologies Used

- **Frontend**: Blazor WebAssembly (C#)
- **Backend**: C# (.NET 8.0)
- **Database**: MySQL 8.0+
- **State Management**: Blazored LocalStorage & SessionStorage
- **Authentication**: Custom ASP.NET Identity
- **Styling**: Pure CSS (no external frameworks)
- **Architecture**: Object-Oriented Programming with SOLID principles

## 📚 Documentation

For comprehensive documentation, see [PROJECT_DOCUMENTATION.md](PROJECT_DOCUMENTATION.md), which includes:

- Detailed application overview
- Complete functionality descriptions
- Challenges faced and solutions
- Implementation details for all components
- Security measures explained
- State management and performance optimization
- Setup and deployment instructions

## 🐛 Troubleshooting

### Database Connection Issues

**Problem**: Cannot connect to MySQL database

**Solution**:
1. Verify MySQL is running: `mysql --version`
2. Check connection string in `DatabaseService.cs`
3. Ensure database exists: `CREATE DATABASE Shop;`
4. Verify credentials are correct

### Build Errors

**Problem**: Build fails with package errors

**Solution**:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Authentication Issues

**Problem**: Cannot login or register

**Solution**:
1. Clear browser cache and local storage
2. Check browser console for errors
3. Verify database connection
4. Ensure Users table exists

### Cart Not Persisting

**Problem**: Cart items disappear on refresh

**Solution**:
1. Check browser local storage permissions
2. Verify database connection
3. Check console for errors
4. Clear local storage and try again

## 🚀 Deployment

### Production Deployment Steps

1. **Update configuration**:
   - Use environment variables for connection strings
   - Enable HTTPS
   - Configure CORS policies

2. **Build for production**:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

3. **Deploy to web server**:
   - IIS, Nginx, or Apache
   - Configure reverse proxy
   - Set up SSL certificate

4. **Database migration**:
   - Backup existing data
   - Run migration scripts
   - Verify data integrity

## 📝 Assignment Requirements Checklist

### Part 1: Core Business Logic ✅
- [x] Blazor WebAssembly project created
- [x] Product class with all required properties
- [x] Product details printing method
- [x] Cart class with List<Product>
- [x] AddProduct method with database integration
- [x] RemoveProduct method with database integration
- [x] DisplayCartItems method
- [x] CalculateTotal method
- [x] Test program demonstrating functionality

### Part 2: Blazor Components ✅
- [x] ProductCard.razor component created
- [x] Component displays product details
- [x] "Add to Cart" button implemented
- [x] Event handling for cart operations
- [x] Multiple products displayed and tested

### Part 3: Styling & Responsiveness ✅
- [x] Custom CSS in wwwroot/css/site.css
- [x] Improved text readability
- [x] Proper spacing and layout
- [x] Responsive design with media queries
- [x] Mobile, tablet, and desktop views
- [x] Accessibility features (color contrast, keyboard navigation)

### Part 4: Security ✅
- [x] Input validation implemented
- [x] SQL injection prevention (parameterized queries)
- [x] XSS prevention (HTML encoding)
- [x] User authentication with ASP.NET Identity
- [x] Protected routes requiring login
- [x] Password hashing (SHA256)

### Part 5: State Management ✅
- [x] Session storage for authentication
- [x] Local storage for cart persistence
- [x] Cart persists after page refresh
- [x] Database persistence for cart items
- [x] Final testing completed
- [x] All components integrated and working

## 🤝 Contributing

This is an educational project demonstrating e-commerce application development with Blazor and C#.

## 📄 License

This project is created for educational purposes.

## 👥 Author

ShopEase Development Team

## 📞 Support

For issues or questions, please refer to the PROJECT_DOCUMENTATION.md file for detailed information about the application architecture, implementation, and troubleshooting.

---

**Version**: 1.0  
**Last Updated**: 2024  
**Built with**: Blazor WebAssembly, C#, MySQL