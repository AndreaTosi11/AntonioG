using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWorkDemo.Models;
using ProjectWorkDemo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ScaffoldingProva.Controllers
{
    [Authorize(Roles ="Admin")]
    public class FornitoriController : Controller
    {
        private readonly ProjectWorkDatabaseContext _context;
        private readonly SignInManager<ProjectWorkDemoUser> _signInManager;
        private readonly UserManager<ProjectWorkDemoUser> _userManager;
        private readonly IUserStore<ProjectWorkDemoUser> _userStore;
        private readonly IUserEmailStore<ProjectWorkDemoUser> _emailStore;

        public FornitoriController(ProjectWorkDatabaseContext context, UserManager<ProjectWorkDemoUser> userManager,
            IUserStore<ProjectWorkDemoUser> userStore,
            SignInManager<ProjectWorkDemoUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;            
        }

        
        [HttpGet]
        public async Task<IActionResult> Index(string Fornsearch, string sortingforn, string currentFilter, int? pageNumber)
        {            
            ViewData["CurrentSort"] = sortingforn;
            ViewData["Getforndetails"] = Fornsearch;
            ViewData["Fornitoriname"] = String.IsNullOrEmpty(sortingforn) ? "NomeDitta_desc" : "";
            var fornquery = from x in _context.Fornitori.Include(f => f.User) select x;
            //PAGINAZIONE
            if (Fornsearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                Fornsearch = currentFilter;
            }
            int pageSize = 5;
            if (pageNumber < 1) { pageNumber = 1; }
            //ORDINAMENTO
            switch (sortingforn)
            {
                case "NomeDitta_desc":
                    fornquery = fornquery.OrderByDescending(x => x.NomeDitta);
                    break;
                default:
                    fornquery = fornquery.OrderBy(x => x.NomeDitta);
                    break;
            }
            //RICERCA
            if (!string.IsNullOrEmpty(Fornsearch))
            {
                fornquery = fornquery.Where(x => x.NomeDitta.Contains(Fornsearch));
                //return View(await fornquery.AsNoTracking().ToListAsync());
            }
            return View(await PaginatedList<Fornitori>.CreateAsync(fornquery.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Fornitoris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fornitori == null)
            {
                return NotFound();
            }

            var fornitori = await _context.Fornitori
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.IdFornitori == id);

            string? queryUserid = (from a in _context.Fornitori
                                 where a.IdFornitori == id
                                 select a.UserId).FirstOrDefault();

            string? queryRoleid = (from a in _context.AspNetUserRoles
                                 where a.UserId == queryUserid
                                 select a.RoleId).FirstOrDefault();

            string? roleName = (from a in _context.AspNetRoles
                                   where a.Id == queryRoleid
                                select a.Name).FirstOrDefault();


            if (fornitori == null)
            {
                return NotFound();
            }
            ViewBag.RoleName = roleName;
            ViewBag.Latitudine = fornitori.Latitudine;
            ViewBag.Longitudine = fornitori.Longitudine;
            return View(fornitori);

        }

        // GET: Fornitoris/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Name");
            return View();
        }

        // POST: Fornitoris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Fornitori fornitori, FornitoriRegister fornitoreRegister, AspNetUserRole userRole)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Nome = fornitoreRegister.Nome;
                user.Cognome = fornitoreRegister.Cognome;
                user.Telefono = fornitoreRegister.Telefono;
                await _userStore.SetUserNameAsync(user, fornitoreRegister.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, fornitoreRegister.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, fornitoreRegister.Password);

                if (result.Succeeded)
                {
                    fornitori = new Fornitori()
                    {
                        UserId = user.Id,
                        DataCreazione = fornitoreRegister.DataCreazione,
                        NomeDitta = fornitoreRegister.NomeDitta,
                        Telefono = fornitoreRegister.Telefono,
                        Indirizzo = fornitoreRegister.Indirizzo,
                        Piva = fornitoreRegister.Piva,
                        Latitudine = fornitoreRegister.Latitudine,
                        Longitudine = fornitoreRegister.Longitudine
                    };

                    userRole = new AspNetUserRole()
                    {
                        UserId = user.Id,
                        RoleId = fornitoreRegister.RoleId
                    };

                    _context.Add(userRole);
                    _context.Add(fornitori);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }


            ViewData["RoleId"] = new SelectList(_context.AspNetUsers, "Id", "Nome");
            return View(fornitori);
        }

        public ProjectWorkDemoUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ProjectWorkDemoUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ProjectWorkDemoUser)}'. " +
                    $"Ensure that '{nameof(ProjectWorkDemoUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ProjectWorkDemoUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ProjectWorkDemoUser>)_userStore;
        }

        // GET: Fornitoris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fornitori == null)
            {
                return NotFound();
            }

            var fornitori = await _context.Fornitori.FindAsync(id);
            if (fornitori == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", fornitori.UserId);
            return View(fornitori);
        }

        // POST: Fornitoris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFornitori,UserId,NomeDitta,DataCreazione,Telefono,Indirizzo,Piva,Latitudine,Longitudine")] Fornitori fornitori)
        {
            if (id != fornitori.IdFornitori)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fornitori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornitoriExists(fornitori.IdFornitori))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", fornitori.UserId);
            return View(fornitori);
        }


        // GET: Fornitoris/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fornitori == null)
            {
                return NotFound();
            }
            //.Include(f => f.IdTipoFornNavigation)
            var fornitori = await _context.Fornitori
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.IdFornitori == id);
            if (fornitori == null)
            {
                return NotFound();
            }

            return View(fornitori);
        }

        // POST: Fornitoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fornitori == null && _context.AspNetUsers == null)
            {
                return Problem("Entity set 'ProjectWorkDatabaseContext Fornitori and AspNetUsers'  are null.");
            }
            else
            {
                var fornitori = await _context.Fornitori.FindAsync(id);

                string? queryUser = (from a in _context.Fornitori
                                    where a.IdFornitori == id
                                    select a.UserId).FirstOrDefault();

                var user = await _context.AspNetUsers.FindAsync(queryUser);

                if (fornitori != null && user != null)
                {
                    _context.Fornitori.Remove(fornitori);
                    _context.AspNetUsers.Remove(user);
                }

                await _context.SaveChangesAsync();
               
            }
                 return RedirectToAction(nameof(Index));
        }

        private bool FornitoriExists(int id)
        {
            return (_context.Fornitori?.Any(e => e.IdFornitori == id)).GetValueOrDefault();
        }
    }
}
