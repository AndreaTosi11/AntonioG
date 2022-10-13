using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWorkDemo.Models;

namespace ControllerLavorazioni.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LavorazioneController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;

        public LavorazioneController(ProjectWorkDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexSelectByFornitore(int? id)
        {

            var projectWorkDatabaseContext = _context.Lavorazione.Include(l => l.IdFornitoriNavigation).Include(l => l.IdMezzoNavigation).Where(m => m.IdFornitori == id);
            return View(await projectWorkDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> IndexSelectByTarga(int? id)
        {
            
            var projectWorkDatabaseContext = _context.Lavorazione.Include(l => l.IdFornitoriNavigation).Include(l => l.IdMezzoNavigation).Where(m => m.IdMezzo == id); 
            return View(await projectWorkDatabaseContext.ToListAsync());
        }

        // GET: Lavoraziones
        public async Task<IActionResult> Index(string Lavsearch, string sortinglav, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortinglav;
            ViewData["Getlavdetails"] = Lavsearch;
            ViewData["Lavtarga"] = String.IsNullOrEmpty(sortinglav) ? "IdMezzoNavigation_desc" : "";
            ViewData["Lavdate"] = sortinglav == "DataLavorazione" ? "DataLavorazione_desc" : "DataLavorazione";
            var lavquery = from x in _context.Lavorazione.Include(l => l.IdFornitoriNavigation).Include(l => l.IdMezzoNavigation) select x;
            if (Lavsearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                Lavsearch = currentFilter;
            }
            switch (sortinglav)
            {
                case "IdMezzoNavigation_desc":
                    lavquery = lavquery.OrderByDescending(x => x.IdMezzoNavigation);
                    break;
                case "DataLavorazione":
                    lavquery = lavquery.OrderBy(x => x.DataLavorazione);
                    break;
                case "DataLavorazione_desc":
                    lavquery = lavquery.OrderByDescending(x => x.DataLavorazione);
                    break;
                default:
                    lavquery = lavquery.OrderBy(x => x.IdMezzoNavigation);
                    break;
            }
            if (!string.IsNullOrEmpty(Lavsearch))
            {
                lavquery = lavquery.Where(x => x.IdMezzoNavigation.Targa.Contains(Lavsearch));
            }
            //return View(await lavquery.AsNoTracking().ToListAsync());
            int pageSize = 5;
            if (pageNumber < 1) { pageNumber = 1; }
            return View(await PaginatedList<Lavorazione>.CreateAsync(lavquery.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Lavoraziones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lavorazione == null)
            {
                return NotFound();
            }

            var lavorazione = await _context.Lavorazione
                .Include(l => l.IdFornitoriNavigation)
                .Include(l => l.IdMezzoNavigation)
                .FirstOrDefaultAsync(m => m.IdLavorazione == id);
            if (lavorazione == null)
            {
                return NotFound();
            }

            return View(lavorazione);
        }

        // GET: Lavoraziones/Create
        public IActionResult Create()
        {
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta");
            ViewData["IdMezzo"] = new SelectList(_context.Mezzo, "IdMezzo", "Targa");
            return View();
        }

        // POST: Lavoraziones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLavorazione,IdFornitori,IdMezzo,CodiceIdentificativo,DataLavorazione,Garanzia,NumGaranzia")] Lavorazione lavorazione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lavorazione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", lavorazione.IdFornitori);
            ViewData["IdMezzo"] = new SelectList(_context.Mezzo, "IdMezzo", "Targa", lavorazione.IdMezzo);
            return View(lavorazione);
        }

        // GET: Lavoraziones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lavorazione == null)
            {
                return NotFound();
            }

            var lavorazione = await _context.Lavorazione.FindAsync(id);
            if (lavorazione == null)
            {
                return NotFound();
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", lavorazione.IdFornitori);
            ViewData["IdMezzo"] = new SelectList(_context.Mezzo, "IdMezzo", "Targa", lavorazione.IdMezzo);
            return View(lavorazione);
        }

        // POST: Lavoraziones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLavorazione,IdFornitori,IdMezzo,CodiceIdentificativo,DataLavorazione,Garanzia,NumGaranzia")] Lavorazione lavorazione)
        {
            if (id != lavorazione.IdLavorazione)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lavorazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LavorazioneExists(lavorazione.IdLavorazione))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", lavorazione.IdFornitori);
            ViewData["IdMezzo"] = new SelectList(_context.Mezzo, "IdMezzo", "Targa", lavorazione.IdMezzo);
            return View(lavorazione);
        }

        // GET: Lavoraziones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lavorazione == null)
            {
                return NotFound();
            }

            var lavorazione = await _context.Lavorazione
                .Include(l => l.IdFornitoriNavigation)
                .Include(l => l.IdMezzoNavigation)
                .FirstOrDefaultAsync(m => m.IdLavorazione == id);
            if (lavorazione == null)
            {
                return NotFound();
            }

            return View(lavorazione);
        }

        // POST: Lavoraziones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lavorazione == null)
            {
                return Problem("Entity set 'ProjectWorkDatabaseContext.Lavorazione'  is null.");
            }
            var lavorazione = await _context.Lavorazione.FindAsync(id);
            if (lavorazione != null)
            {
                _context.Lavorazione.Remove(lavorazione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LavorazioneExists(int id)
        {
          return (_context.Lavorazione?.Any(e => e.IdLavorazione == id)).GetValueOrDefault();
        }
    }
}
