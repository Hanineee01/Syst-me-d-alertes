using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TTronAlert.Desktop.Configuration;
using TTronAlert.Desktop.Services;
using TTronAlert.Desktop.ViewModels;
using TTronAlert.Desktop.Views;

namespace TTronAlert.Desktop;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }
    public static TrayIcon? AppTrayIcon { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();

        // Add configuration
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<AlertSystemSettings>(configuration.GetSection("AlertSystem"));

        // Add services
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton<IToastNotificationService, ToastNotificationService>();
        services.AddSingleton<MainWindowViewModel>();

        Services = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
        {
            // Tray-only app (no MainWindow)

            // Create tray icon programmatically
            var tray = new TrayIcon();
            tray.Icon = CreateTrayIcon();
            tray.ToolTipText = "Connecting...";
            tray.Clicked += (s, e) =>
            {
                // No main window - click could show a quick status toast
                var toastService = Services?.GetRequiredService<IToastNotificationService>();
                var alertService = Services?.GetRequiredService<IAlertService>();
                if (toastService != null && alertService != null)
                {
                    // Create a small informational toast using a dummy AlertDto
                    var status = alertService.IsConnected ? "Connected" : "Disconnected";
                    var dto = new TTronAlert.Shared.DTOs.AlertDto(0, "Status", status, 0, DateTime.UtcNow, false, false, null);
                    toastService.ShowToast(dto);
                }
            };

            // Register the tray icon with the application (attached property)
            try
            {
                var icons = new TrayIcons();
                icons.Add(tray);
                TrayIcon.SetIcons(this, icons);
                AppTrayIcon = tray;
            }
            catch
            {
                // Fallback: keep a reference only
                AppTrayIcon = tray;
            }

            var alertServiceMain = Services?.GetRequiredService<IAlertService>();
            var toastSvc = Services?.GetRequiredService<IToastNotificationService>();

            if (alertServiceMain != null)
            {
                // Show toast when alert received
                if (toastSvc != null)
                {
                    alertServiceMain.AlertReceived += (s, alert) =>
                    {
                        toastSvc.ShowToast(alert);
                    };
                }

                // Update tray tooltip on connection state change
                alertServiceMain.ConnectionStateChanged += (s, connected) =>
                {
                    if (AppTrayIcon != null)
                    {
                        AppTrayIcon.ToolTipText = connected ? "Connected" : "Disconnected";
                    }
                };

                // Start service (fire-and-forget)
                _ = alertServiceMain.StartAsync();

                // Set initial state
                if (AppTrayIcon != null)
                {
                    AppTrayIcon.ToolTipText = alertServiceMain.IsConnected ? "Connected" : "Disconnected";
                }
            }

            // Keep application headless (tray-only)
            // Do not set desktop.MainWindow
            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private WindowIcon? CreateTrayIcon()
    {
        try
        {
            var logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.png");
            if (System.IO.File.Exists(logoPath))
            {
                var fileInfo = new System.IO.FileInfo(logoPath);
                if (fileInfo.Length <= 1024 * 1024)
                {
                    return new WindowIcon(new Bitmap(logoPath));
                }
            }

            var fallbackBitmap = new RenderTargetBitmap(new PixelSize(32, 32), new Vector(96, 96));
            using (var context = fallbackBitmap.CreateDrawingContext())
            {
                context.DrawEllipse(Brushes.DodgerBlue, new Pen(Brushes.White, 2), new Point(16, 16), 12, 12);
                context.DrawLine(new Pen(Brushes.White, 3), new Point(16, 8), new Point(16, 18));
                context.DrawEllipse(Brushes.White, null, new Point(16, 22), 1.5, 1.5);
            }
            return new WindowIcon(fallbackBitmap);
        }
        catch
        {
            return null;
        }
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
