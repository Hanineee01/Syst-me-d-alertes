using Hardcodet.Wpf.TaskbarNotification;
using ClientAlertesWPF.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using CommunityToolkit.WinUI.Notifications; // <-- Nouveau package
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ClientAlertesWPF.Services
{
    public class AlertService
    {
        private readonly TaskbarIcon _tb;
        private HubConnection? _connection;
        private readonly HttpClient _http = new HttpClient();
        private readonly int _machineId = Environment.MachineName.GetHashCode();
        private readonly string _logoPath;

        public AlertService(TaskbarIcon tb)
        {
            _tb = tb;
            _logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "logo.png");
        }

        public async void Start()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/alerthub")
                .Build();

            _connection.On<string>("ReceiveAlert", json =>
            {
                var vm = JsonConvert.DeserializeObject<AlertViewModel>(json);
                if (vm != null) ShowToast(vm);
            });

            try
            {
                await _connection.StartAsync();
                _tb.ToolTipText = "Alertes : Temps réel OK";
            }
            catch
            {
                _ = PollingLoop();
                _tb.ToolTipText = "Alertes : Polling (5s)";
            }
        }

        private async Task PollingLoop()
        {
            while (true)
            {
                try
                {
                    var response = await _http.GetAsync($"http://localhost:5000/api/alerts/pending/{_machineId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var alerts = JsonConvert.DeserializeObject<List<AlertViewModel>>(json);
                        foreach (var vm in alerts ?? new List<AlertViewModel>())
                            ShowToast(vm);
                    }
                }
                catch { }

                await Task.Delay(5000);
            }
        }

        private void ShowToast(AlertViewModel vm)
        {
            new ToastContentBuilder()
                .AddAppLogoOverride(new Uri(_logoPath), ToastGenericAppLogoCrop.Circle)
                .AddHeader(vm.Level, vm.Title, "")
                .AddText(vm.Message)
                .AddButton(new ToastButton("OK", "ack").SetBackgroundActivation())
                .SetToastScenario(vm.Level == "Critical" ? ToastScenario.Alarm : ToastScenario.Default)
                .AddAudio(vm.Level == "Critical"
                    ? new ToastAudio { Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm"), Loop = true }
                    : new ToastAudio { Src = new Uri("ms-winsoundevent:Notification.Default") })
                .Show();
        }

        public void Stop()
        {
            _connection?.StopAsync().Wait();
        }
    }
}