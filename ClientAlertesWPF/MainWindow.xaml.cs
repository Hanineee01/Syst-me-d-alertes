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
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}