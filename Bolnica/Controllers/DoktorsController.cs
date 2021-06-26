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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Bolnica.Controllers
{
    public class DoktorsController : Controller
    {
        private readonly BolnicaContext _context;
        private readonly IHostingEnvironment webHostEnvironment;

        public DoktorsController(BolnicaContext context, IHostingEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Doktors
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Doktor> doktori = _context.Doktor.AsEnumerable();
            doktori = _context.Doktor.Include(m => m.Pacients).ThenInclude(m => m.Pacient);

            if (!string.IsNullOrEmpty(searchString))
            {
                doktori = doktori.Where(s => s.FullName.Contains(searchString));
            }


            var prom = new DoktorImeViewModel
            {
                Doktors = doktori.ToList()
            };
            return View(prom);
            /*
            var pacienti = _context.Doktor.Include(m => m.Pacients).ThenInclude(m => m.Pacient);
            return View(pacienti);*/
        }

        // GET: Doktors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktor.Include(m => m.Pacients).ThenInclude(m => m.Pacient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doktor == null)
            {
                return NotFound();
            }

            return View(doktor);
        }

        // GET: Doktors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doktors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoktorViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Doktor doktor = new Doktor
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    Zvanje = model.Zvanje,
                    Vozrast = model.Vozrast,
                    DataVrabotuvanje = model.DataVrabotuvanje,
                    Plata = model.Plata,
                    ProfilnaSlika = uniqueFileName,
                };

                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(DoktorViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfilnaSlika != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilnaSlika.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilnaSlika.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Doktors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var doktor = _context.Doktor.Where(m => m.Id == id).Include(m => m.Pacients).First();
            //var doktor = await _context.Doktor.FindAsync(id);
            if (doktor == null)
            {
                return NotFound();
            }

            var pacient = _context.Pacient.AsEnumerable();
            pacient = pacient.OrderBy(s => s.FullName);
            DoktorPacientViewModel viewmodel = new DoktorPacientViewModel
            {
                Doktor = doktor,
                PacientList = new MultiSelectList(pacient, "Id", "FullName"),
                SelectedPacients = doktor.Pacients.Select(sa => sa.PacientId)
            };

            return View(viewmodel);
        }

        // POST: Doktors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoktorPacientViewModel viewmodel)
        {
            if (id != viewmodel.Doktor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Doktor);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listPacient = viewmodel.SelectedPacients;
                    IQueryable<LekuvanPacient> toBeRemoved = _context.LekuvanPacient.Where(s => !listPacient.Contains(s.PacientId) && s.DoktorId == id);
                    _context.LekuvanPacient.RemoveRange(toBeRemoved);
                    IEnumerable<int> existPacient = _context.LekuvanPacient.Where(s => listPacient.Contains(s.PacientId) && s.DoktorId == id).Select(s => s.PacientId);
                    IEnumerable<int> newPacient = listPacient.Where(s => !existPacient.Contains(s));
                    foreach (int pacientId in newPacient)
                        _context.LekuvanPacient.Add(new LekuvanPacient { PacientId = pacientId, DoktorId = id });

                    //_context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoktorExists(viewmodel.Doktor.Id))
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
            return View(viewmodel);
        }

        // GET: Doktors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktor.Include(m => m.Pacients).ThenInclude(m => m.Pacient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doktor == null)
            {
                return NotFound();
            }

            return View(doktor);
        }

        // POST: Doktors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doktor = await _context.Doktor.FindAsync(id);
            _context.Doktor.Remove(doktor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoktorExists(int id)
        {
            return _context.Doktor.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Pacienti(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacient = await _context.Doktor.Include(m=>m.Pacients).ThenInclude(m=>m.Pacient)
                .FirstOrDefaultAsync(m => m.Id == id);

            var doktor = await _context.Doktor.FindAsync(id);

            ViewData["Doktor"] = doktor.FullName;
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
