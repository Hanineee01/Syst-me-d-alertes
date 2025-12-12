using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlertesApi.Data;
using AlertesApi.Models;
using Microsoft.AspNetCore.SignalR;
using AlertesApi.Hubs;
using Newtonsoft.Json;

namespace AlertesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertesController : ControllerBase
    {
        private readonly AlertesContext _context;
        private readonly IHubContext<AlertesHub> _hubContext;

        public AlertesController(AlertesContext context, IHubContext<AlertesHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Alertes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alerte>>> GetAlertes()
        {
            return await _context.Alertes.ToListAsync();
        }

        // GET: api/Alertes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alerte>> GetAlerte(int id)
        {
            var alerte = await _context.Alertes.FindAsync(id);
            if (alerte == null)
            {
                return NotFound();
            }
            return alerte;
        }

        // POST: api/Alertes
        [HttpPost]
        public async Task<ActionResult<Alerte>> PostAlerte(Alerte alerte)
        {
            alerte.DateCreation = DateTime.UtcNow;
            _context.Alertes.Add(alerte);
            await _context.SaveChangesAsync();

            // ENVOI EN TEMPS RÉEL AU FORMAT CORRECT DU CLIENT
            var alertForClient = new
            {
                Title = alerte.Titre,
                Message = alerte.Message,
                Level = alerte.Niveau
            };
            await _hubContext.Clients.All.SendAsync("ReceiveAlert", JsonConvert.SerializeObject(alertForClient));

            return CreatedAtAction(nameof(GetAlerte), new { id = alerte.Id }, alerte);
        }

        // PUT: api/Alertes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlerte(int id, Alerte alerte)
        {
            if (id != alerte.Id)
            {
                return BadRequest();
            }
            _context.Entry(alerte).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlerteExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            var alertForClient = new
            {
                Title = alerte.Titre,
                Message = alerte.Message,
                Level = alerte.Niveau
            };
            await _hubContext.Clients.All.SendAsync("ReceiveAlert", JsonConvert.SerializeObject(alertForClient));

            return NoContent();
        }

        // DELETE: api/Alertes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlerte(int id)
        {
            var alerte = await _context.Alertes.FindAsync(id);
            if (alerte == null)
            {
                return NotFound();
            }
            _context.Alertes.Remove(alerte);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // NOUVEAU : POST api/Alertes/machines/register
        [HttpPost("machines/register")]
        public async Task<IActionResult> RegisterPoste(Poste poste)
        {
            var existing = await _context.Postes.FirstOrDefaultAsync(p => p.TokenUnique == poste.TokenUnique);
            if (existing == null)
            {
                _context.Postes.Add(poste);
            }
            else
            {
                existing.Nom = poste.Nom;
                existing.DerniereConnexion = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // NOUVEAU : GET api/Alertes/alerts/pending/{machineId}
        [HttpGet("alerts/pending/{machineId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingAlertes(string machineId)
        {
            var pending = await _context.Alertes
                .Where(a => !a.EstLue && (a.PosteIdDestinataire == null || a.PosteIdDestinataire.ToString() == machineId))
                .Select(a => new
                {
                    Title = a.Titre,
                    Message = a.Message,
                    Level = a.Niveau
                })
                .ToListAsync();

            return Ok(pending);
        }

        private bool AlerteExists(int id)
        {
            return _context.Alertes.Any(e => e.Id == id);
        }
    }
}