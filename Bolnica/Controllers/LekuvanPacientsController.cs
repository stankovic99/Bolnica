using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bolnica.Data;
using Bolnica.Models;

namespace Bolnica.Controllers
{
    public class LekuvanPacientsController : Controller
    {
        private readonly BolnicaContext _context;

        public LekuvanPacientsController(BolnicaContext context)
        {
            _context = context;
        }

        // GET: LekuvanPacients
        public async Task<IActionResult> Index()
        {
            var bolnicaContext = _context.LekuvanPacient.Include(l => l.Doktor).Include(l => l.Pacient);
            return View(await bolnicaContext.ToListAsync());
        }

        // GET: LekuvanPacients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lekuvanPacient = await _context.LekuvanPacient
                .Include(l => l.Doktor)
                .Include(l => l.Pacient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lekuvanPacient == null)
            {
                return NotFound();
            }

            return View(lekuvanPacient);
        }

        // GET: LekuvanPacients/Create
        public IActionResult Create()
        {
            ViewData["DoktorId"] = new SelectList(_context.Doktor, "Id", "FullName");
            ViewData["PacientId"] = new SelectList(_context.Pacient, "Id", "FullName");
            return View();
        }

        // POST: LekuvanPacients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Lek,PacientId,DoktorId")] LekuvanPacient lekuvanPacient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lekuvanPacient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoktorId"] = new SelectList(_context.Doktor, "Id", "FullName", lekuvanPacient.DoktorId);
            ViewData["PacientId"] = new SelectList(_context.Pacient, "Id", "FullName", lekuvanPacient.PacientId);
            return View(lekuvanPacient);
        }

        // GET: LekuvanPacients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lekuvanPacient = await _context.LekuvanPacient.FindAsync(id);
            var pacient = await _context.Pacient.FindAsync(lekuvanPacient.PacientId);
            var doktor = await _context.Doktor.FindAsync(lekuvanPacient.DoktorId);
            ViewData["Pacient"] = pacient.FullName;
            ViewData["Doktor"] = doktor.FullName;
            if (lekuvanPacient == null)
            {
                return NotFound();
            }
            ViewData["DoktorId"] = new SelectList(_context.Doktor, "Id", "FullName", lekuvanPacient.DoktorId);
            ViewData["PacientId"] = new SelectList(_context.Pacient, "Id", "FullName", lekuvanPacient.PacientId);
            return View(lekuvanPacient);
        }

        // POST: LekuvanPacients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Lek,PacientId,DoktorId")] LekuvanPacient lekuvanPacient)
        {
            if (id != lekuvanPacient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lekuvanPacient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LekuvanPacientExists(lekuvanPacient.Id))
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
            ViewData["DoktorId"] = new SelectList(_context.Doktor, "Id", "FullName", lekuvanPacient.DoktorId);
            ViewData["PacientId"] = new SelectList(_context.Pacient, "Id", "FullName", lekuvanPacient.PacientId);
            return View(lekuvanPacient);
        }

        // GET: LekuvanPacients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lekuvanPacient = await _context.LekuvanPacient
                .Include(l => l.Doktor)
                .Include(l => l.Pacient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lekuvanPacient == null)
            {
                return NotFound();
            }

            return View(lekuvanPacient);
        }

        // POST: LekuvanPacients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lekuvanPacient = await _context.LekuvanPacient.FindAsync(id);
            _context.LekuvanPacient.Remove(lekuvanPacient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LekuvanPacientExists(int id)
        {
            return _context.LekuvanPacient.Any(e => e.Id == id);
        }
    }
}
