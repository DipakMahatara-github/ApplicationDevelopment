using Microsoft.Extensions.Logging;
using ApplicationDevelopment.Service;

namespace ApplicationDevelopment;

// Entry point for configuring and building the .NET MAUI application
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Create a MAUI application builder
        var builder = MauiApp.CreateBuilder();

        builder
            // Register the main application class
            .UseMauiApp<App>()
            // Configure custom fonts used in the application
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Enable Blazor WebView to run Blazor components inside MAUI app
        builder.Services.AddMauiBlazorWebView();

        // Register PDF export service as a singleton for dependency injection
        builder.Services.AddSingleton<PdfExportService>();

#if DEBUG
        // Enable developer tools and debug logging in development mode
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Build and return the configured MAUI application
        return builder.Build();
    }
}