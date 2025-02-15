using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegalCaseManagement.Data;
using LegalCaseManagement.Models;

namespace LegalCaseManagement.Controllers
{
    public class LegalCasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LegalCasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LegalCases
        public async Task<IActionResult> Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var query = _context.LegalCases.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(lc => lc.CaseTitle.Contains(searchTerm) || lc.Description.Contains(searchTerm));
            }

            var totalCases = await query.CountAsync();
            var cases = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCases / pageSize);
            ViewData["CurrentPage"] = page;
            ViewData["SearchTerm"] = searchTerm;

            return View(cases);
        }

        // GET: LegalCases/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: LegalCases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTitle,Description,CaseDate,Status")] LegalCase legalCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(legalCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(legalCase);
        }

        // GET: LegalCases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var legalCase = await _context.LegalCases.FindAsync(id); if (legalCase == null)
            {
                return NotFound();
            }
            return View(legalCase);
        }

        // POST: LegalCases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaseTitle,Description,CaseDate,Status")] LegalCase legalCase)
        {
            if (id != legalCase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(legalCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LegalCaseExists(legalCase.Id))
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
            return View(legalCase);
        }

        // GET: LegalCases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var legalCase = await _context.LegalCases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (legalCase == null)
            {
                return NotFound();
            }

            return View(legalCase);
        }
        // POST: LegalCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var legalCase = await _context.LegalCases.FindAsync(id);
            _context.LegalCases.Remove(legalCase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LegalCaseExists(int id)
        {
            return _context.LegalCases.Any(e => e.Id == id);
        }
    }
}

