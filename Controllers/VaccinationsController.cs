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
using hadasimExe1new.Models;

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


        public async Task<IActionResult> Summary()
        {
            var summaryData = new Dictionary<string, object>();

            // Count customers with no vaccinations
            var unvaccinatedCustomersCount = await _context.Client
                .CountAsync(c => !_context.Vaccination.Any(v => v.MemberId == c.Id));
            summaryData["UnvaccinatedCustomersCount"] = unvaccinatedCustomersCount;

            // Calculate the start date and end date of the last month
            var startDate = DateTime.Today.AddMonths(-1).Date;
            var endDate = DateTime.Today.Date;

            // Retrieve all clients with sickness dates in the last month
            var activePatientsInLastMonth = await _context.Client
                .Where(c => c.DateOfSickness >= startDate && c.DateOfSickness <= endDate)
                .ToListAsync();

            // Group active patients by date of sickness in memory
            var activePatientsByDate = activePatientsInLastMonth
                .GroupBy(c => c.DateOfSickness.Value.Date)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count());
            summaryData["ActivePatientsByDate"] = activePatientsByDate;

            return View(summaryData);
        }

        //Receives a client ID and returns the list of vaccinations related only to him

        public async Task<IActionResult> ViewVaccinations(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            var vaccinations = await _context.Vaccination
                                                .Where(v => v.MemberId == client.Id)
                                                .ToListAsync();

            return View("Index", vaccinations);
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
                    ModelState.AddModelError("DateVaccination", "The new date cannot be earlier than existing vaccination dates.");
                    return View(vaccination);
                }
                var lastVacine = vacine.OrderByDescending(v => v.DateVaccination).FirstOrDefault();
                if (lastVacine != null && lastVacine.DateVaccination > vaccination.DateVaccination)
                {
                    ModelState.AddModelError("DateVaccination", "There is no possibility of more than four vaccinations per client");
                    return View(vaccination);
                }
                _context.Add(vaccination);

                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Clients", new { id = vaccination.MemberId });
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
                // Retrieve existing vaccinations for the same member
                var existingVaccinations = await _context.Vaccination
                                                        .Where(v => v.MemberId == vaccination.MemberId && v.id != id)
                                                        .OrderBy(v => v.DateVaccination)
                                                        .ToListAsync();

                // Check if the new date conflicts with existing dates
                foreach (var existingVaccination in existingVaccinations)
                {
                    if (vaccination.DateVaccination < existingVaccination.DateVaccination)
                    {
                        ModelState.AddModelError("DateVaccination", "The new date cannot be earlier than existing vaccination dates.");
                        return View(vaccination);
                    }
                }

                // Save changes if no conflicts found
                try
                {
                    _context.Update(vaccination);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
