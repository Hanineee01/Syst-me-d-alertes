using System;
using TTronAlert.Shared.DTOs;
using TTronAlert.Shared.Models;
namespace TTronAlert.Desktop.ViewModels;
public class AlertItemViewModel : ViewModelBase
{
    public int Id { get; }
    public string Title { get; }
    public string Message { get; }
    public AlertLevel Level { get; }
    public DateTime CreatedAt { get; }
    public string TimeAgo { get; }
    public string LevelText { get; }
    public string LevelClass { get; }

    // Ajout des propriétés booléennes pour les classes conditionnelles
    public bool IsInfo => Level == AlertLevel.Info;
    public bool IsWarning => Level == AlertLevel.Warning;
    public bool IsCritical => Level == AlertLevel.Critical;

    public AlertItemViewModel(AlertDto alert)
    {
        Id = alert.Id;
        Title = alert.Title;
        Message = alert.Message;
        Level = alert.Level;
        CreatedAt = alert.CreatedAt;
        TimeAgo = GetTimeAgo(alert.CreatedAt);
        LevelText = alert.Level.ToString();
        LevelClass = alert.Level.ToString().ToLower();
    }
    private static string GetTimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;
        if (timeSpan.TotalMinutes < 1)
            return "Just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes}m ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours}h ago";
        return $"{(int)timeSpan.TotalDays}d ago";
    }
}