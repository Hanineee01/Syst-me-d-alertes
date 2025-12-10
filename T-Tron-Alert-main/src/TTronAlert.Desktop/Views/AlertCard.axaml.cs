using Avalonia.Controls;
using TTronAlert.Desktop.ViewModels;

namespace TTronAlert.Desktop.Views;

public partial class AlertCard : UserControl
{
    public AlertCard()
    {
        InitializeComponent();
        
        DataContextChanged += OnDataContextChanged;
    }
    
    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is AlertItemViewModel alert)
        {
            var border = this.FindControl<Border>("AlertBorder");
            if (border != null)
            {
                border.Classes.Add(alert.LevelClass);
            }
        }
    }
}
