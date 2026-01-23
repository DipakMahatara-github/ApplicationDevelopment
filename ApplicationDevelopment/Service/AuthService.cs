using Microsoft.Maui.Storage;

namespace ApplicationDevelopment.Service
{
    public static class AuthService
    {
        private const string PinKey = "user_pin";
        private const string UsernameKey = "username";
        private static bool _isUnlocked = false;

        public static void SaveUsername(string username)
        {
            Preferences.Set(UsernameKey, username.Trim());
        }

        public static string GetUsername()
        {
            return Preferences.Get(UsernameKey, "");
        }

        public static async Task SavePinAsync(string pin)
        {
            pin = pin.Trim();
            try
            {
                SecureStorage.Remove(PinKey);
                await SecureStorage.SetAsync(PinKey, pin);
                Preferences.Set(PinKey, pin);
            }
            catch
            {
                Preferences.Set(PinKey, pin);
            }
        }

        public static async Task<string?> GetPinAsync()
        {
            try
            {
                var pin = await SecureStorage.GetAsync(PinKey);
                if (!string.IsNullOrEmpty(pin))
                    return pin.Trim();
            }
            catch { }

            var fallback = Preferences.Get(PinKey, "");
            return string.IsNullOrEmpty(fallback) ? null : fallback.Trim();
        }

        public static async Task<bool> HasPinAsync()
        {
            return !string.IsNullOrEmpty(await GetPinAsync());
        }

        public static async Task<bool> ValidatePinAsync(string enteredPin)
        {
            var storedPin = await GetPinAsync();
            if (!string.IsNullOrEmpty(storedPin) && storedPin == enteredPin.Trim())
            {
                _isUnlocked = true;
                return true;
            }
            return false;
        }

        public static bool IsUnlocked() => _isUnlocked;

        public static void Lock() => _isUnlocked = false;
    }
}