namespace TTronAlert.Shared.Models;

public class Alert
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public AlertLevel Level { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
}
