using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using TTronAlert.Desktop.ViewModels;

namespace TTronAlert.Desktop.Views
{
    public partial class AlertToastWindow : Window
    {
        public AlertToastWindow()
        {
            InitializeComponent();
            // Fermeture automatique après 15 secondes
            DispatcherTimer.RunOnce(CloseWindow, TimeSpan.FromSeconds(15));

            // Ajout pour démarrer l'animation à l'ouverture
            Opened += OnOpened;
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            ToastBorder.Opacity = 1;
            if (ToastBorder.RenderTransform is TranslateTransform transform)
            {
                transform.X = 0;
            }
        }

        public void SetAlert(AlertItemViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void CloseWindow()
        {
            Dispatcher.UIThread.Post(() => this.Close());
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PositionToast(int index)
        {
            // Logique pour positionner le toast (par exemple, en bas à droite, empilés)
            var screen = Screens.ScreenFromVisual(this);
            if (screen != null)
            {
                var workArea = screen.WorkingArea;
                Position = new PixelPoint(workArea.Right - (int)Width - 20, workArea.Bottom - (int)Height - 20 - (index * ((int)Height + 10)));
            }
        }
    }
}