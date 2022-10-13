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
    public class ServiziFornitoreController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;

        public ServiziFornitoreController(ProjectWorkDatabaseContext context)
        {
            _context = context;
        }

        // GET: ServiziFornitores
        public async Task<IActionResult> IndexSelectByNomeDitta(int? id, int? idForn)
        {
            var projectWorkDatabaseContext = _context.ServiziFornitore.Include(l => l.IdFornitoriNavigation).Include(l => l.IdTipoFornNavigation).Where(m => m. IdFornitori == id || m.IdFornitori == idForn);
            return View(await projectWorkDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> Index(string Servsearch, string sortingserv, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortingserv;
            ViewData["Getservdetails"] = Servsearch;
            ViewData["Servizitype"] = sortingserv == "IdTipoFornNavigation" ? "IdTipoFornNavigation_desc" : "IdTipoFornNavigation";
            //ViewData["Servizitype"] = String.IsNullOrEmpty(sortingserv) ? "IdTipoFornNavigation_desc" : "IdTipoFornNavigation";
            var servquery = from x in _context.ServiziFornitore.Include(s => s.IdFornitoriNavigation).Include(s => s.IdTipoFornNavigation) select x;
            if (Servsearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                Servsearch = currentFilter;
            }
            switch (sortingserv)
            {
                case "IdTipoFornNavigation_desc":
                    servquery = servquery.OrderBy(x => x.IdTipoFornNavigation);
                    break;
                case "IdTipoFornNavigation":
                    servquery = servquery.OrderByDescending(x => x.IdTipoFornNavigation);
                    break;
                default:
                    servquery = servquery.OrderBy(x => x.IdFornitoriNavigation);
                    break;
            }
            if (!string.IsNullOrEmpty(Servsearch))
            {
                servquery = servquery.Where(x => x.IdTipoFornNavigation.Descrizione.Contains(Servsearch) || x.IdFornitoriNavigation.NomeDitta.Contains(Servsearch));
            }

            //return View(await servquery.AsNoTracking().ToListAsync());
            int pageSize = 5; 
            if (pageNumber < 1) { pageNumber = 1; }
            return View(await PaginatedList<ServiziFornitore>.CreateAsync(servquery.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: ServiziFornitores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiziFornitore == null)
            {
                return NotFound();
            }

            var serviziFornitore = await _context.ServiziFornitore
                .Include(s => s.IdFornitoriNavigation)
                .Include(s => s.IdTipoFornNavigation)
                .FirstOrDefaultAsync(m => m.IdServizio == id);
            if (serviziFornitore == null)
            {
                return NotFound();
            }

            return View(serviziFornitore);
        }

        // GET: ServiziFornitores/Create
        public IActionResult Create(int? id)
        {           
            ViewBag.IdFornitori = id;
            string? nomeDitta = (from a in _context.Fornitori
                              where a.IdFornitori == id
                              select a.NomeDitta).FirstOrDefault();

            ViewBag.NomeDitta = nomeDitta;
            ViewData["IdTipoForn"] = new SelectList(_context.TipoForn, "IdTipoForn", "Descrizione");
            return View();
        }

        // POST: ServiziFornitores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdServizio,IdFornitori,IdTipoForn")] ServiziFornitore serviziFornitore,int id)
        {
            
            if (ModelState.IsValid)
            {

                var servizio = _context.ServiziFornitore.Include(s => s.IdFornitoriNavigation).Include(s => s.IdTipoFornNavigation).Where(a => a.IdFornitori == serviziFornitore.IdFornitori && a.IdTipoForn == serviziFornitore.IdTipoForn).ToList();
                if (servizio.Count == 0)
                {
                    _context.Add(serviziFornitore);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Fornitori");
                }
                else
                {
                    ViewBag.ServizioExist = " Il servizio non è stato aggiunto perché già esiste ";
                    ViewBag.IdFornitori = id;
                    string? nomeDitta = (from a in _context.Fornitori
                                         where a.IdFornitori == id
                                         select a.NomeDitta).FirstOrDefault();

                    ViewBag.NomeDitta = nomeDitta;
                    ViewData["IdTipoForn"] = new SelectList(_context.TipoForn, "IdTipoForn", "Descrizione");
                    return View();

                }
               
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", serviziFornitore.IdFornitori);
            ViewData["IdTipoForn"] = new SelectList(_context.TipoForn, "IdTipoForn", "Descrizione", serviziFornitore.IdTipoForn);
            return RedirectToAction("Index", "Fornitori"); 
        }

        // GET: ServiziFornitores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiziFornitore == null)
            {
                return NotFound();
            }

            var serviziFornitore = await _context.ServiziFornitore.FindAsync(id);
            if (serviziFornitore == null)
            {
                return NotFound();
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", serviziFornitore.IdFornitori);
            ViewData["IdTipoForn"] = new SelectList(_context.TipoForn, "IdTipoForn", "Descrizione", serviziFornitore.IdTipoForn);
            return View(serviziFornitore);
        }

        // POST: ServiziFornitores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("IdServizio,IdFornitori,IdTipoForn")] ServiziFornitore serviziFornitore)
        {
            int idForn = serviziFornitore.IdFornitori;
            if (id != serviziFornitore.IdServizio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviziFornitore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiziFornitoreExists(serviziFornitore.IdServizio))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexSelectByNomeDitta", "ServiziFornitore", new { idForn });
            }
            ViewData["IdFornitori"] = new SelectList(_context.Fornitori, "IdFornitori", "NomeDitta", serviziFornitore.IdFornitori);
            ViewData["IdTipoForn"] = new SelectList(_context.TipoForn, "IdTipoForn", "Descrizione", serviziFornitore.IdTipoForn);
            return View(serviziFornitore);
        }

        // GET: ServiziFornitores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.ServiziFornitore == null)
            {
                return NotFound();
            }

            var serviziFornitore = await _context.ServiziFornitore
                .Include(s => s.IdFornitoriNavigation)
                .Include(s => s.IdTipoFornNavigation)
                .FirstOrDefaultAsync(m => m.IdServizio == id);

            if (serviziFornitore == null)
            {
                return NotFound();
            }

            return View(serviziFornitore);
        }

        // POST: ServiziFornitores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {           
            if (_context.ServiziFornitore == null)
            {
                return Problem("Entity set 'ProjectWorkDatabaseContext.ServiziFornitore'  is null.");
            }
            var serviziFornitore = await _context.ServiziFornitore.FindAsync(id);
            int idForn = serviziFornitore.IdFornitori;
            if (serviziFornitore != null)
            {
                
                _context.ServiziFornitore.Remove(serviziFornitore);
            }
           
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexSelectByNomeDitta", "ServiziFornitore", new { idForn });
        }

        private bool ServiziFornitoreExists(int id)
        {
          return (_context.ServiziFornitore?.Any(e => e.IdServizio == id)).GetValueOrDefault();
        }
    }
}
