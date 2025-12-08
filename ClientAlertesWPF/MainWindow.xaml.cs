using CommunityToolkit.WinUI.Notifications;
using System.Windows;

namespace ClientAlertesWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Test toast simple au démarrage
            ToastNotificationManagerCompat.OnActivated += toastArgs => { };

            new ToastContentBuilder()
                .AddText("Test toast – si tu vois ça, les toasts marchent !")
                .AddButton(new ToastButton("OK", "test"))
                .Show();

            // Cache la fenêtre
            this.Visibility = Visibility.Hidden;
        }
    }
}