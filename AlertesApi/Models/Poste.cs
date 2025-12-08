using System;

namespace AlertesApi.Models
{
    public class Poste
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string TokenUnique { get; set; } = Guid.NewGuid().ToString();
        public DateTime DerniereConnexion { get; set; } = DateTime.UtcNow;
    }
}