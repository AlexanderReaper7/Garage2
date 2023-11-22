using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models.Entities;

namespace Garage2.Controllers
{
    public class MembersController : Controller
    {
        private readonly Garage2Context _context;

        public MembersController(Garage2Context context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
              return _context.Member != null ? 
                          View(await _context.Member.ToListAsync()) :
                          Problem("Entity set 'Garage2Context.Member'  is null.");
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }
            // ERR: needs to include ParkedVehicle.VehicleType
            var member = await _context.Member.Include(m => m.ParkedVehicle).ThenInclude(pv => pv.VehicleType)
                .FirstOrDefaultAsync(m => m.PersonNumber == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonNumber,FirstName,LastName,Membership,ParkedVehicleId")] Member member)
        {
            if (ModelState.IsValid)
            {
                // Normalise the person number
                switch (member.PersonNumber.Length)
                {
                    // Determine if the input is in the format YYMMDD-XXXX or YYYYMMDD-XXXX, and if the - is missing or replaced with a +
                    case 11: // YYMMDD-XXXX or YYMMDD+XXXX
                        // if the - has been replaced with +, then the person is going to be >= 100 years old this year
                        if (member.PersonNumber.ElementAt(6) == '+')
                        {
                            member.PersonNumber = member.PersonNumber.Replace("+", "");
                            // get current year - 100
                            int currentYear = DateTime.Now.Year - 100;
                            member.PersonNumber = currentYear.ToString().Substring(0, 2) + member.PersonNumber;
                        }
                        else if (member.PersonNumber.ElementAt(6) == '-')
                        {
                            // strip the '-' from the input
                            member.PersonNumber = member.PersonNumber.Replace("-", "");
                        }
                        else
                        {
                            ModelState.AddModelError("PersonNumber", "Invalid format");
                            return View(member);
                        }
                        break;
                    case 10: // YYMMDDXXXX
                        // check if the person was born this century or the last
                        if (int.Parse(member.PersonNumber.Substring(0, 2)) > DateTime.Now.Year - 2000)
                            // if the person was born this century, add 20 to the year
                            member.PersonNumber = "20" + member.PersonNumber.Substring(0, 2) + member.PersonNumber.Substring(2, 8);
                        else
                            // if the person was born last century, add 19 to the year
                            member.PersonNumber = "19" + member.PersonNumber.Substring(0, 2) + member.PersonNumber.Substring(2, 8);

                        member.PersonNumber = DateTime.Now.Year.ToString().Substring(0, 2) + member.PersonNumber;
                        break;
                    case 13: // YYYYMMDD-XXXX or YYYYMMDD+XXXX
                        // strip the + or -
                        member.PersonNumber = member.PersonNumber.Replace("+", "").Replace("-", "");
                        break;
                    case 12: // YYYYMMDDXXXX
                        break;
                }

                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonNumber,FirstName,LastName,Membership,ParkedVehicleId")] Member member)
        {
            if (id != member.PersonNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.PersonNumber))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.PersonNumber == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Member == null)
            {
                return Problem("Entity set 'Garage2Context.Member'  is null.");
            }
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
          return (_context.Member?.Any(e => e.PersonNumber == id)).GetValueOrDefault();
        }
    }
}
