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
    public class MezzoController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;

        public MezzoController(ProjectWorkDatabaseContext context)
        {
            _context = context;
        }

        // GET: Mezzo
        [HttpGet]
        public async Task<IActionResult> Index(string Mezzosearch, string sortingmezzo, string currentFilter, int? pageNumber)
        {
            //var projectWorkDatabaseContext = _context.Mezzo.Include(m => m.IdProduttoreNavigation);
            //return View(await projectWorkDatabaseContext.ToListAsync());
            ViewData["CurrentSort"] = sortingmezzo;
            ViewData["Getmezzodetails"] = Mezzosearch;
            ViewData["Mezziname"] = String.IsNullOrEmpty(sortingmezzo) ? "TipoAuto_desc" : "";
            ViewData["Mezzidate"] = sortingmezzo == "DataImmatricolazione" ? "DataImmatricolazione_desc" : "DataImmatricolazione";
            var mezzoquery = from x in _context.Mezzo.Include(m => m.IdProduttoreNavigation) select x;
            if (Mezzosearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                Mezzosearch = currentFilter;
            }
            switch (sortingmezzo)
            {
                case "TipoAuto_desc":
                    mezzoquery = mezzoquery.OrderByDescending(x => x.TipoAuto);
                    break;
                case "DataImmatricolazione":
                    mezzoquery = mezzoquery.OrderBy(x => x.DataImmatricolazione);
                    break;
                case "DataImmatricolazione_desc":
                    mezzoquery = mezzoquery.OrderByDescending(x => x.DataImmatricolazione);
                    break;
                default:
                    mezzoquery = mezzoquery.OrderBy(x => x.TipoAuto);
                    break;
            }
            if (!string.IsNullOrEmpty(Mezzosearch))
            {
                mezzoquery = mezzoquery.Where(x => x.TipoAuto.Contains(Mezzosearch) || x.Targa.Contains(Mezzosearch));
            }
            int pageSize = 5;
            if (pageNumber < 1) { pageNumber = 1; } 
            return View(await PaginatedList<Mezzo>.CreateAsync(mezzoquery.AsNoTracking(), pageNumber ?? 1, pageSize)); 
        }

        // GET: Mezzo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mezzo == null)
            {
                return NotFound();
            }

            var mezzo = await _context.Mezzo
                .Include(m => m.IdProduttoreNavigation)
                .FirstOrDefaultAsync(m => m.IdMezzo == id);
            if (mezzo == null)
            {
                return NotFound();
            }

            return View(mezzo);
        }

        // GET: Mezzo/Create
        public IActionResult Create()
        {
            ViewData["IdProduttore"] = new SelectList(_context.Produttore, "IdProduttore", "Denominazione");
            return View();
        }

        // POST: Mezzo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMezzo,IdProduttore,TipoAuto,Targa,DataImmatricolazione")] Mezzo mezzo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mezzo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProduttore"] = new SelectList(_context.Produttore, "IdProduttore", "Denominazione", mezzo.IdProduttore);
            return View(mezzo);
        }

        // GET: Mezzo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mezzo == null)
            {
                return NotFound();
            }

            var mezzo = await _context.Mezzo.FindAsync(id);
            if (mezzo == null)
            {
                return NotFound();
            }
            ViewData["IdProduttore"] = new SelectList(_context.Produttore, "IdProduttore", "Denominazione", mezzo.IdProduttore);
            return View(mezzo);
        }

        // POST: Mezzo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMezzo,IdProduttore,TipoAuto,Targa,DataImmatricolazione")] Mezzo mezzo)
        {
            if (id != mezzo.IdMezzo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mezzo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MezzoExists(mezzo.IdMezzo))
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
            ViewData["IdProduttore"] = new SelectList(_context.Produttore, "IdProduttore", "Denominazione", mezzo.IdProduttore);
            return View(mezzo);
        }

        // GET: Mezzo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mezzo == null)
            {
                return NotFound();
            }

            var mezzo = await _context.Mezzo
                .Include(m => m.IdProduttoreNavigation)
                .FirstOrDefaultAsync(m => m.IdMezzo == id);
            if (mezzo == null)
            {
                return NotFound();
            }

            return View(mezzo);
        }

        // POST: Mezzo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mezzo == null)
            {
                return Problem("Entity set 'ProjectWorkDatabaseContext.Mezzo'  is null.");
            }
            var mezzo = await _context.Mezzo.FindAsync(id);
            if (mezzo != null)
            {
                _context.Mezzo.Remove(mezzo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MezzoExists(int id)
        {
          return (_context.Mezzo?.Any(e => e.IdMezzo == id)).GetValueOrDefault();
        }
    }
}
