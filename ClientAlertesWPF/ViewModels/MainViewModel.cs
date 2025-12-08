<<<<<<< HEAD
<<<<<<< HEAD
using Microsoft.AspNetCore.SignalR.Client;
=======
<<<<<<< HEAD
﻿using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.AspNetCore.SignalR.Client;
=======
﻿using Microsoft.AspNetCore.SignalR.Client;
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
=======
﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
>>>>>>> parent of 6205666 (Design)
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows;
<<<<<<< HEAD
<<<<<<< HEAD
using Hardcodet.Wpf.TaskbarNotification;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
=======
<<<<<<< HEAD
=======
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
=======
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
>>>>>>> parent of 6205666 (Design)

namespace ClientAlertesWPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
<<<<<<< HEAD
<<<<<<< HEAD
        private HubConnection connection;
        private readonly TaskbarIcon trayIcon;

        public MainViewModel()
        {
            trayIcon = (TaskbarIcon)Application.Current.FindResource("NotifyIcon");

            // TON LOGO SWITCH DANS LA BARRE DES TÂCHES
            LoadLogoFromUrlAsync();

            ConnectToSignalR();
        }

        private async void LoadLogoFromUrlAsync()
        {
            try
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync("https://switchcompagnie.eu/assets/img/logo.png");
                using var ms = new MemoryStream(bytes);
                using var bitmap = new Bitmap(ms);
                trayIcon.Icon = Icon.FromHandle(bitmap.GetHicon());
            }
            catch
            {
                trayIcon.Icon = System.Drawing.SystemIcons.Shield;
            }
=======
<<<<<<< HEAD
        private readonly TaskbarIcon tray;

        public MainViewModel()
        {
            tray = (TaskbarIcon)Application.Current.FindResource("MyNotifyIcon");

            MessageBox.Show("CLIENT LANCÉ – ICÔNE DEVRAIT ÊTRE VISIBLE", "TEST", MessageBoxButton.OK, MessageBoxImage.Information);

            Connect();
        }

        private async void Connect()
        {
            var conn = new HubConnectionBuilder()
                .WithUrl("https://localhost:7012/hubs/alertes", o =>
                {
                    o.HttpMessageHandlerFactory = (_) => new HttpClientHandler
=======
        private HubConnection connection;

        public MainViewModel()
        {
            ConnectToSignalR();
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
=======
        public RelayCommand OpenCommand { get; }
        public RelayCommand ShowWindowCommand { get; }
        public RelayCommand ExitCommand { get; }

        private readonly TaskbarIcon notifyIcon;

        public MainViewModel()
        {
            OpenCommand = new RelayCommand(() => MessageBox.Show("Système d'alertes actif"));
            ShowWindowCommand = new RelayCommand(() => MessageBox.Show("Système d'alertes actif"));
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());

            notifyIcon = (TaskbarIcon)Application.Current.FindResource("NotifyIcon");
            notifyIcon.Icon = System.Drawing.SystemIcons.Shield; // icône bleue bouclier (toujours visible)

            ConnectToServer();
>>>>>>> parent of 6205666 (Design)
        }

        private async void ConnectToServer()
        {
<<<<<<< HEAD
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7012/hubs/alertes", options =>
                {
                    options.HttpMessageHandlerFactory = (_) => new HttpClientHandler
<<<<<<< HEAD
=======
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                })
<<<<<<< HEAD
=======
<<<<<<< HEAD
                .Build();

            conn.On<string, string, string>("ReceiveAlerte", (titre, message, niveau) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (niveau == "Critique")
                    {
                        for (int i = 0; i < 15; i++)
                            SystemSounds.Hand.Play();
                    }

                    tray.ShowBalloonTip(titre, message, BalloonIcon.Info);
                });
=======
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
=======
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5177/hubs/alertes")
>>>>>>> parent of 6205666 (Design)
                .WithAutomaticReconnect()
                .Build();

            connection.On<Alerte>("ReceiveAlerte", (alerte) =>
            {
<<<<<<< HEAD
                Application.Current.Dispatcher.Invoke(() => ShowSwitchToast(alerte));
<<<<<<< HEAD
=======
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
=======
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Son qui hurle à mort
                    for (int i = 0; i < 10; i++)
                    {
                        SystemSounds.Hand.Play();
                    }

                    // Toast rouge si critique
                    BalloonIcon icon = alerte.Niveau == "Critique" ? BalloonIcon.Error : BalloonIcon.Warning;

                    notifyIcon.ShowBalloonTip(
                        alerte.Titre,
                        alerte.Message,
                        icon);
                });
>>>>>>> parent of 6205666 (Design)
            });

            try
            {
<<<<<<< HEAD
=======
<<<<<<< HEAD
                await conn.StartAsync();
                tray.ToolTipText = "Connecté";
                tray.ShowBalloonTip("CONNECTÉ", "Le poste est prêt", BalloonIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR : " + ex.Message);
=======
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
                await connection.StartAsync();
<<<<<<< HEAD
                trayIcon.ToolTipText = "Switch Alertes · Connecté";
                ShowSwitchToast(new Alerte { Titre = "Connecté", Message = "Le poste est prêt à recevoir les alertes.", Niveau = "Info" });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion échouée : " + ex.Message);
<<<<<<< HEAD
=======
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
            }
        }

        private void ShowSwitchToast(Alerte alerte)
        {
            string backgroundColor = alerte.Niveau switch
            {
                "Critique" => "#FF4B4B",
                "Avertissement" => "#FF8C42",
                _ => "#3777FF"
            };

            string sound = alerte.Niveau switch
            {
                "Critique" => "ms-winsoundevent:Notification.SMS",
                "Avertissement" => "ms-winsoundevent:Notification.Reminder",
                _ => "ms-winsoundevent:Notification.Mail"
            };

            string xml = $@"<?xml version='1.0'?>
<toast duration='long' scenario='{(alerte.Niveau == "Critique" ? "alarm" : "default")}'>
    <visual>
        <binding template='ToastGeneric'>
            <image placement='appLogoOverride' hint-crop='circle' src='https://switchcompagnie.eu/assets/img/logo.png'/>
            <text hint-style='title' color='#4A0E80'>{alerte.Titre}</text>
            <text hint-style='body' color='#FFFFFF'>{alerte.Message}</text>
            <text placement='attribution' color='#FF2B80'>
                Poste : {Environment.MachineName} • {DateTime.Now:HH:mm}
            </text>
        </binding>
    </visual>
    <audio src='{sound}' loop='{(alerte.Niveau == "Critique" ? "true" : "false")}'/>
    <actions>
        <action content='Marquer comme lue' arguments='read:{alerte.Id}' activationType='protocol'/>
        <action content='Fermer' arguments='close' activationType='system'/>
    </actions>
</toast>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var toast = new ToastNotification(doc);
            toast.Data = new NotificationData();
            toast.Data.Values["backgroundColor"] = backgroundColor;

            ToastNotificationManager.CreateToastNotifier("Système d'alertes Switch Compagnie").Show(toast);
        }
=======
                notifyIcon.ToolTipText = "Alertes · Connecté";
                notifyIcon.ShowBalloonTip("Système d'alertes", "Connecté avec succès !", BalloonIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR CONNEXION :\n" + ex.Message);
            }
        }
>>>>>>> parent of 6205666 (Design)
    }
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96

    public class Alerte
    {
        public int Id { get; set; }
        public string Titre { get; set; } = "";
        public string Message { get; set; } = "";
        public string Niveau { get; set; } = "Info";
        public DateTime DateCreation { get; set; }
        public bool EstLue { get; set; }
        public bool EstArchivee { get; set; }
        public int? PosteIdDestinataire { get; set; }
    }
<<<<<<< HEAD
=======
>>>>>>> 6205666af16cf0d558b6c06f4c92f1cfa67fd098
>>>>>>> 6076090afb5cee47de722d23dac1eacca9271f96
}