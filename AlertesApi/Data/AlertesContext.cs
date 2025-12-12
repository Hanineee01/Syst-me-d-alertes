using Microsoft.EntityFrameworkCore;
using AlertesApi.Models;

namespace AlertesApi.Data
{
    public class AlertesContext : DbContext
    {
        public AlertesContext(DbContextOptions<AlertesContext> options) : base(options) { }

        public DbSet<Alerte> Alertes => Set<Alerte>();
        public DbSet<Poste> Postes => Set<Poste>();
        public DbSet<Acknowledgement> Acknowledgements => Set<Acknowledgement>();
    }
}