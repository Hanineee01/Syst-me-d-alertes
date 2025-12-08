using ClientAlertesWPF.Services;
using Hardcodet.Wpf.TaskbarNotification;
using CommunityToolkit.WinUI.Notifications;
using System;
using System.Windows;

namespace ClientAlertesWPF
{
    public partial class App : Application
    {
        private TaskbarIcon? tb;
        private AlertService? alertService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ligne magique pour les toasts en .NET 8 WPF
            ToastNotificationManagerCompat.OnActivated += toastArgs => { };

            tb = new TaskbarIcon
            {
                ToolTipText = "Système d'alertes actif"
            };

            // Logo chargé depuis les ressources (marche à 100 %)
            var iconUri = new Uri("pack://application:,,,/Resources/Icons/logo.ico");
            tb.Icon = new System.Drawing.Icon(Application.GetResourceStream(iconUri)!.Stream);

            // Menu Quitter
            var contextMenu = new System.Windows.Controls.ContextMenu();
            var quitItem = new System.Windows.Controls.MenuItem { Header = "Quitter" };
            quitItem.Click += (s, a) => Current.Shutdown();
            contextMenu.Items.Add(quitItem);
            tb.ContextMenu = contextMenu;

            alertService = new AlertService(tb);
            alertService.Start();

            MainWindow = new MainWindow
            {
                WindowState = WindowState.Minimized,
                ShowInTaskbar = false,
                Visibility = Visibility.Hidden
            };
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            tb?.Dispose();
            alertService?.Stop();
            base.OnExit(e);
        }
    }
}