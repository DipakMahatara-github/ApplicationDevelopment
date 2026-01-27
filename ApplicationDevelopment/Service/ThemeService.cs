using Microsoft.Maui.Storage;

namespace ApplicationDevelopment.Service
{
    public static class ThemeService
    {
        private const string ThemeKey = "app_theme";

        public static string GetTheme()
        {
            return Preferences.Get(ThemeKey, "light");
        }

        public static void SetTheme(string theme)
        {
            Preferences.Set(ThemeKey, theme);
        }

        public static bool IsDarkMode()
        {
            return GetTheme() == "dark";
        }

        public static void ToggleTheme()
        {
            var newTheme = IsDarkMode() ? "light" : "dark";
            SetTheme(newTheme);
        }
    }
}