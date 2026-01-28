using Microsoft.Maui.Storage;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationDevelopment.Service
{
    // Service responsible for handling authentication logic (register, login, logout)
    public static class AuthService
    {
        // Keys used to store authentication data in local preferences
        private const string UsernameKey = "app_username";
        private const string PasswordKey = "app_password";
        private const string LoginKey = "app_logged_in";

        // Event triggered when authentication state changes (login/logout)
        public static event Action? OnAuthStateChanged;

        // Notify subscribed components about authentication state change
        private static void Notify() => OnAuthStateChanged?.Invoke();

        // =========================
        // USER REGISTRATION
        // =========================
        public static Task RegisterAsync(string username, string password)
        {
            // Store username after trimming whitespace
            Preferences.Set(UsernameKey, username.Trim());

            // Store hashed password instead of plain text for security
            Preferences.Set(PasswordKey, Hash(password));

            // Ensure user is logged out after registration (no auto-login)
            Logout();
            return Task.CompletedTask;
        }

        // =========================
        // USER LOGIN
        // =========================
        public static Task<bool> LoginAsync(string username, string password)
        {
            // Retrieve stored credentials
            var savedUser = Preferences.Get(UsernameKey, "");
            var savedPass = Preferences.Get(PasswordKey, "");

            // Validate credentials by comparing username and hashed password
            if (savedUser == username.Trim() && savedPass == Hash(password))
            {
                // Mark user as logged in
                Preferences.Set(LoginKey, true);

                // Notify UI components about login state change
                Notify();
                return Task.FromResult(true);
            }

            // Return false if authentication fails
            return Task.FromResult(false);
        }

        // =========================
        // USER LOGOUT
        // =========================
        public static void Logout()
        {
            // Update login state to false
            Preferences.Set(LoginKey, false);

            // Notify UI components about logout state change
            Notify();
        }

        // Checks whether the user is currently logged in
        public static bool IsLoggedIn()
        {
            return Preferences.Get(LoginKey, false);
        }

        // Hashes the password using SHA-256 encryption for security
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
