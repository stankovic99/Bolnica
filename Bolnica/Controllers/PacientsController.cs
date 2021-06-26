using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bolnica.Data;
using Bolnica.Models;
using Bolnica.ViewModels;

namespace Bolnica.Controllers
{
    public class PacientsController : Controller
    {
        private readonly BolnicaContext _context;

        public PacientsController(BolnicaContext context)
        {
            _context = context;
        }

        // GET: Pacients
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Pacient> pacient = _context.Pacient.AsEnumerable();

            if (!string.IsNullOrEmpty(searchString))
            {
                pacient = pacient.Where(s => s.FullName.Contains(searchString));
            }

            var proba = new PacientViewModel
            {
                Pacienti = pacient.ToList()
            };

            return View(proba);
        }

        // GET: Pacients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacient = await _context.Pacient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacient == null)
            {
                return NotFound();
            }

            return View(pacient);
        }

        // GET: Pacients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,PrethodniBolesti,PriemenDatum,Vozrast")] Pacient pacient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pacient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pacient);
        }

        // GET: Pacients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacient = await _context.Pacient.FindAsync(id);
            if (pacient == null)
            {
                return NotFound();
            }
            return View(pacient);
        }

        // POST: Pacients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,PrethodniBolesti,PriemenDatum,Vozrast")] Pacient pacient)
        {
            if (id != pacient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacientExists(pacient.Id))
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
            return View(pacient);
        }

        // GET: Pacients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacient = await _context.Pacient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacient == null)
            {
                return NotFound();
            }

            return View(pacient);
        }

        // POST: Pacients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pacient = await _context.Pacient.FindAsync(id);
            _context.Pacient.Remove(pacient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacientExists(int id)
        {
            return _context.Pacient.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Doktori(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacient = await _context.Pacient.Include(m=>m.Doktors).ThenInclude(m=>m.Doktor)
                .FirstOrDefaultAsync(m => m.Id == id);

            var doktor = await _context.Pacient.FindAsync(id);

            ViewData["Pacient"] = doktor.FullName;
            if (pacient == null)
            {
                return NotFound();
            }

            return View(pacient);
        }
        public async Task<IActionResult> Lekovi(int? id)
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
        public async Task<IActionResult> Lekovi(int id, [Bind("Id,Lek,PacientId,DoktorId")] LekuvanPacient lekuvanPacient)
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
        private bool LekuvanPacientExists(int id)
        {
            return _context.LekuvanPacient.Any(e => e.Id == id);
        }
    }
}
