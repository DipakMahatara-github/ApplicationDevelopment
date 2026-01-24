using Microsoft.Maui.Storage;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationDevelopment.Service
{
    public static class AuthService
    {
        private const string UsernameKey = "app_username";
        private const string PasswordKey = "app_password";
        private const string LoginKey = "app_logged_in";

        public static event Action? OnAuthStateChanged;

        private static void Notify() => OnAuthStateChanged?.Invoke();

        // ✅ SET ACCOUNT (REGISTER)
        public static Task RegisterAsync(string username, string password)
        {
            Preferences.Set(UsernameKey, username.Trim());
            Preferences.Set(PasswordKey, Hash(password));

            Logout(); // important: do NOT auto login
            return Task.CompletedTask;
        }

        // ✅ LOGIN
        public static Task<bool> LoginAsync(string username, string password)
        {
            var savedUser = Preferences.Get(UsernameKey, "");
            var savedPass = Preferences.Get(PasswordKey, "");

            if (savedUser == username.Trim() && savedPass == Hash(password))
            {
                Preferences.Set(LoginKey, true);
                Notify();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        // ✅ LOGOUT
        public static void Logout()
        {
            Preferences.Set(LoginKey, false);
            Notify();
        }

        public static bool IsLoggedIn()
        {
            return Preferences.Get(LoginKey, false);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}