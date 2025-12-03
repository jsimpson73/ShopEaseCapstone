using System;

namespace ShopEase.Models
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Validates user data
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Email) &&
                   Email.Contains("@");
        }
    }
}