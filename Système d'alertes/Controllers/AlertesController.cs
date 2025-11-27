using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlertesApi.Data;
using AlertesApi.Models;

namespace AlertesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertesController : ControllerBase
    {
        private readonly AlertesContext _context;

        public AlertesController(AlertesContext context)
        {
            _context = context;
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
            _context.Alertes.Add(alerte);
            await _context.SaveChangesAsync();

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

        private bool AlerteExists(int id)
        {
            return _context.Alertes.Any(e => e.Id == id);
        }
    }
}