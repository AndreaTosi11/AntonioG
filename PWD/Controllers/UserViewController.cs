using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWorkDemo.Models;

namespace ProjectWorkDemo.Controllers
{
    public class UserViewController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;

        public UserViewController(ProjectWorkDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> AnagraficaFornitore()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? queryFornitore = (from a in _context.Fornitori
                                 where a.UserId == userId
                                 select a.IdFornitori).FirstOrDefault();

            var fornitori = await _context.Fornitori
               .FirstOrDefaultAsync(m => m.IdFornitori == queryFornitore);

            if (fornitori == null)
            {
                return NotFound();
            }
            ViewBag.Latitudine = fornitori.Latitudine;
            ViewBag.Longitudine = fornitori.Longitudine;
            return View(fornitori);
        }
        public IActionResult ElencoAuto()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? Queryid = (from a in _context.Fornitori
                                   where a.UserId == userId
                                   select a.IdFornitori).FirstOrDefault();

            var mezzi = _context.GetMezziById.FromSqlRaw($"GetMezziById {Queryid}").ToList();

            return View(mezzi);
        }

        public IActionResult ElencoLavorazioni()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? queryFornitore = (from a in _context.Fornitori
                                   where a.UserId == userId
                                   select a.IdFornitori).FirstOrDefault();

            var QueryIdForn  = (from a in _context.Lavorazione
                                   where a.IdFornitori == queryFornitore
                                      select a.IdFornitori).FirstOrDefault();

            var lavorazioni  = _context.Lavorazione.Include(c => c.IdMezzoNavigation).Include(d => d.IdFornitoriNavigation).Where(m => m.IdFornitori == QueryIdForn);
            return View(lavorazioni);

        }

        public async Task<IActionResult> ElencoDettagli(int? id, int? idFornitore)
        {
            var projectWorkDatabaseContext = _context.Dettaglio.Include(d => d.IdLavorazioneNavigation).Where(m => m.IdLavorazione == id);
            ViewBag.IdFornitore = idFornitore;
            return View(await projectWorkDatabaseContext.ToListAsync());
        }


    }
}
