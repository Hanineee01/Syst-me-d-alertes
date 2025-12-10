using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
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
        var services = new ServiceCollection();
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton<IToastNotificationService, ToastNotificationService>();
        services.AddSingleton<MainWindowViewModel>();
        Services = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainViewModel = Services?.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            desktop.MainWindow = mainWindow;

            if (TrayIcon.GetIcons(this)?.Count > 0)
            {
                AppTrayIcon = TrayIcon.GetIcons(this)?[0];
                if (AppTrayIcon != null)
                {
                    AppTrayIcon.Icon = CreateTrayIcon();
                    AppTrayIcon.Clicked += (s, e) =>
                    {
                        if (mainWindow.IsVisible)
                            mainWindow.Hide();
                        else
                            mainWindow.Show();
                    };
                }
            }

            var alertService = Services?.GetRequiredService<IAlertService>();
            alertService?.StartAsync();

            desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;
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
