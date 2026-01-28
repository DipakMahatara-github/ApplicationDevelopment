using Microsoft.Maui.Storage;

namespace ApplicationDevelopment.Service
{
    // Service class responsible for managing application theme (light/dark mode)
    public static class ThemeService
    {
        // Key used to store theme preference in local storage
        private const string ThemeKey = "app_theme";

        // Retrieves the saved theme from preferences (default is light mode)
        public static string GetTheme()
        {
            return Preferences.Get(ThemeKey, "light");
        }

        // Saves the selected theme into local preferences
        public static void SetTheme(string theme)
        {
            Preferences.Set(ThemeKey, theme);
        }

        // Checks whether the current theme is dark mode
        public static bool IsDarkMode()
        {
            return GetTheme() == "dark";
        }

        // Toggles between light and dark theme and updates the stored value
        public static void ToggleTheme()
        {
            var newTheme = IsDarkMode() ? "light" : "dark";
            SetTheme(newTheme);
        }
    }
}