using ClientAlertesWPF.ViewModels;
using System.Windows;

namespace ClientAlertesWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide(); // cache la fenêtre mais garde l'icône vivante
        }
    }
}