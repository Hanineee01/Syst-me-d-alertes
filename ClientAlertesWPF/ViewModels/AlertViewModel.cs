using System.Windows.Media;

namespace ClientAlertesWPF.ViewModels
{
    public class AlertViewModel
    {
        public string Title { get; set; } = "Nouvelle alerte";
        public string Message { get; set; } = "Message d'alerte.";
        public string Level { get; set; } = "Info";

        public Brush TitleColor => Level switch
        {
            "Info" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3777FF")),
            "Warning" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C42")),
            "Critical" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4B4B")),
            _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A0E80"))
        };
    }
}