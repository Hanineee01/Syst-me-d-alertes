using ClientAlertesWPF.Services;
using Hardcodet.Wpf.TaskbarNotification;
using CommunityToolkit.WinUI.Notifications;
using System;
using System.IO;
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

            // Ligne obligatoire pour les toasts
            ToastNotificationManagerCompat.OnActivated += toastArgs => { };

            tb = new TaskbarIcon
            {
                ToolTipText = "Système d'alertes actif"
            };

            // Chargement du logo par chemin relatif
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "logo.ico");
            if (File.Exists(iconPath))
            {
                tb.Icon = new System.Drawing.Icon(iconPath);
            }
            else
            {
                tb.ToolTipText += " (logo non trouvé)";
            }

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