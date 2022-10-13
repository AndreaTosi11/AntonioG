using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWorkDemo.Models;

namespace ProjectWorkDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DettaglioController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;

        public DettaglioController(ProjectWorkDatabaseContext context)
        {
            _context = context;
        }

        // GET: Dettaglios
        public async Task<IActionResult> Index(int?id, int?idLavorazione)
        {
            var projectWorkDatabaseContext = _context.Dettaglio.Include(d => d.IdLavorazioneNavigation).Where(m => m.IdLavorazione == id || m.IdLavorazione==idLavorazione);
            return View(await projectWorkDatabaseContext.ToListAsync());
        }

        public async Task<IActionResult> IndexFromFornitori(int? id, int? idFornitore)
        {
            var projectWorkDatabaseContext = _context.Dettaglio.Include(d => d.IdLavorazioneNavigation).Where(m => m.IdLavorazione == id);
            ViewBag.IdFornitore = idFornitore;
            return View(await projectWorkDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> IndexFromMezzi(int? id, int?idMezzo)
        {
            var projectWorkDatabaseContext = _context.Dettaglio.Include(d => d.IdLavorazioneNavigation).Where(m => m.IdLavorazione == id );
            ViewBag.IdMezzo = idMezzo;
            return View(await projectWorkDatabaseContext.ToListAsync());
        }

        // GET: Dettaglios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dettaglio == null)
            {
                return NotFound();
            }

            var dettaglio = await _context.Dettaglio
                .Include(d => d.IdLavorazioneNavigation)
                .FirstOrDefaultAsync(m => m.IdDettaglio == id);
            ViewBag.IdLav = dettaglio.IdLavorazione;
            if (dettaglio == null)
            {
                return NotFound();
            }

            return View(dettaglio);
        }

        // GET: Dettaglios/Create
        public IActionResult Create(int? id)
        {
            ViewBag.IdLavorazione =  id;
            return View();
        }

        // POST: Dettaglios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDettaglio,IdLavorazione,TipoLavorazione,DataInizioLavoro,DataFineLavoro,NomeLavoratore,CognomeLavoratore,CostoNetto,Iva,CostoTotDet")] Dettaglio dettaglio,int id)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(dettaglio);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Dettaglio", new { id });
            }
            return View(dettaglio);
        }

        // GET: Dettaglios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dettaglio == null)
            {
                return NotFound();
            }

            var dettaglio = await _context.Dettaglio.FindAsync(id);        
            ViewBag.IdLav = dettaglio.IdLavorazione;
            if (dettaglio == null)
            {
                return NotFound();
            }
            ViewData["IdLavorazione"] = new SelectList(_context.Lavorazione, "IdLavorazione", "IdLavorazione", dettaglio.IdLavorazione);
            return View(dettaglio);
        }

        // POST: Dettaglios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDettaglio,IdLavorazione,TipoLavorazione,DataInizioLavoro,DataFineLavoro,NomeLavoratore,CognomeLavoratore,CostoNetto,Iva,CostoTotDet")] Dettaglio dettaglio)
        {
            int idLavorazione = dettaglio.IdLavorazione;
            if (id != dettaglio.IdDettaglio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dettaglio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DettaglioExists(dettaglio.IdDettaglio))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Dettaglio", new { idLavorazione });
            }
            ViewData["IdLavorazione"] = new SelectList(_context.Lavorazione, "IdLavorazione", "IdLavorazione", dettaglio.IdLavorazione);
            return View(dettaglio);
        }

        // GET: Dettaglios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dettaglio == null)
            {
                return NotFound();
            }

            var dettaglio = await _context.Dettaglio
                .Include(d => d.IdLavorazioneNavigation)
                .FirstOrDefaultAsync(m => m.IdDettaglio == id);
            ViewBag.IdLav = dettaglio.IdLavorazione;
            if (dettaglio == null)
            {
                return NotFound();
            }

            return View(dettaglio);
        }

        // POST: Dettaglios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if (_context.Dettaglio == null)
            {
                return Problem("Entity set 'ProjectWorkDatabaseContext.Dettaglio'  is null.");
            }
            var dettaglio = await _context.Dettaglio.FindAsync(id);
            int idLavorazione = dettaglio.IdLavorazione;
            if (dettaglio != null)
            {
                _context.Dettaglio.Remove(dettaglio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dettaglio", new { idLavorazione });
        }

        private bool DettaglioExists(int id)
        {
          return (_context.Dettaglio?.Any(e => e.IdDettaglio == id)).GetValueOrDefault();
        }
    }
}
