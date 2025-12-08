using System;

namespace AlertesApi.Models
{
    public class Alerte
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Niveau { get; set; } = "Info"; // Info, Warning, Critical
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public bool EstLue { get; set; } = false;
        public bool EstArchivee { get; set; } = false;
        public int? PosteIdDestinataire { get; set; } // null = tous les postes
    }
}