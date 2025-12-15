namespace TTronAlert.Desktop.Configuration;

public class AlertSystemSettings
{
    public string WorkstationId { get; set; } = "poste-1";
    public string ApiBaseUrl { get; set; } = "http://localhost:62051";
    public string HubPath { get; set; } = "/alerthub";
    public bool AutoReconnect { get; set; } = true;

    public string GetHubUrl()
    {
        return $"{ApiBaseUrl}{HubPath}?workstationId={WorkstationId}";
    }
}
