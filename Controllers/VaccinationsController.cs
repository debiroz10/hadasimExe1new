using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hadasimExe1new.Data;
using hadsimnew.Models;
using System.Reflection.PortableExecutable;

namespace hadasimExe1new.Controllers
{
    public class VaccinationsController : Controller
    {
        private readonly hadasimExe1newContext _context;
       
        public VaccinationsController(hadasimExe1newContext context)
        {
            _context = context;
        }

        // GET: Vaccinations
        public async Task<IActionResult> Index()
        {
              return _context.Vaccination != null ? 
                          View(await _context.Vaccination.ToListAsync()) :
                          Problem("Entity set 'hadasimExe1newContext.Vaccination'  is null.");
        }

        // GET: Vaccinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination
                .FirstOrDefaultAsync(m => m.id == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }


        // GET: Vaccinations/Create
        public IActionResult Create(int memberId)
        {
            ViewData["MemberId"] = memberId;
            return View();
        }


        // POST: Vaccinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*int clientId*/[Bind("id,DateVaccination,manufacturer,MemberId")] Vaccination vaccination)
        {

            if (ModelState.IsValid)
            {
                var member = _context.Client.Find(vaccination.MemberId);
                if (member == null)
                {
                    return NotFound();
                }

                var vacine = _context.Vaccination.Where(v => v.MemberId == vaccination.MemberId).ToList();
                if (vacine.Count() >= 4)
                {
                    return BadRequest("Cannot add more than 4 vaccinations");
                }
                var lastVacine = vacine.OrderByDescending(v => v.DateVaccination).FirstOrDefault();
                if (lastVacine != null && lastVacine.DateVaccination > vaccination.DateVaccination)
                {
                    return BadRequest("The date is wrong");
                }
                _context.Add(vaccination);

                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Clients", new { id = vaccination.id });
            }
            return View(vaccination);
        }

        // GET: Vaccinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination.FindAsync(id);
            if (vaccination == null)
            {
                return NotFound();
            }
            return View(vaccination);
        }

        // POST: Vaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,DateVaccination,manufacturer,MemberId")] Vaccination vaccination)
        {
            if (id != vaccination.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationExists(vaccination.id))
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
            return View(vaccination);
        }

        // GET: Vaccinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination
                .FirstOrDefaultAsync(m => m.id == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // POST: Vaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vaccination == null)
            {
                return Problem("Entity set 'hadasimExe1newContext.Vaccination'  is null.");
            }
            var vaccination = await _context.Vaccination.FindAsync(id);
            if (vaccination != null)
            {
                _context.Vaccination.Remove(vaccination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VaccinationExists(int id)
        {
          return (_context.Vaccination?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
