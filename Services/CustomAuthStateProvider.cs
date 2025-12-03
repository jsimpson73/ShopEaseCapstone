using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;

namespace ShopEase.Services
{
    /// <summary>
    /// Custom authentication state provider for managing user authentication
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly IDatabaseService _databaseService;

        public CustomAuthStateProvider(ISessionStorageService sessionStorage, IDatabaseService databaseService)
        {
            _sessionStorage = sessionStorage;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Gets the current authentication state
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var username = await _sessionStorage.GetItemAsync<string>("username");

            if (string.IsNullOrWhiteSpace(username))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "apiauth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Marks the user as authenticated
        /// </summary>
        public async Task MarkUserAsAuthenticated(string username)
        {
            await _sessionStorage.SetItemAsync("username", username);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "apiauth");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        /// <summary>
        /// Marks the user as logged out
        /// </summary>
        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.RemoveItemAsync("username");

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        /// <summary>
        /// Validates user credentials and logs them in
        /// </summary>
        public async Task<bool> Login(string username, string password)
        {
            // Sanitize inputs
            username = System.Net.WebUtility.HtmlEncode(username.Trim());
            
            var isValid = await _databaseService.ValidateUser(username, password);
            
            if (isValid)
            {
                await MarkUserAsAuthenticated(username);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        public async Task<bool> Register(string username, string email, string password)
        {
            // Sanitize inputs
            username = System.Net.WebUtility.HtmlEncode(username.Trim());
            email = System.Net.WebUtility.HtmlEncode(email.Trim());

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (!email.Contains("@"))
            {
                return false;
            }

            // Check if user already exists
            var existingUser = await _databaseService.GetUserByUsername(username);
            if (existingUser != null)
            {
                return false;
            }

            // Hash password
            var passwordHash = HashPassword(password);

            var user = new Models.User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            return await _databaseService.CreateUser(user);
        }

        /// <summary>
        /// Hashes a password using SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}