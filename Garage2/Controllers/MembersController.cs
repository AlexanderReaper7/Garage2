using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models;
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
        public async Task<IActionResult> Create([Bind("PersonNumber,FirstName,LastName,Membership")] Member member)
        {
            if (!ModelState.IsValid) return View(member);
            try
            {
                var temp = member.PersonNumber;
                PersonNumber.Normalize(ref temp);
                member.PersonNumber = temp;
            }
            catch (Exception)
            {
                ModelState.AddModelError("PersonNumber", "Invalid person number.");
                return View(member);
            }
            if (_context.Member.Any(v => v.PersonNumber == member.PersonNumber))
            {
                ModelState.AddModelError("PersonNumber", "This person is already a member");
                return View(member);
            }
            _context.Add(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(string id, [Bind("PersonNumber,FirstName,LastName,Membership")] Member member)
        {
            if (id != member.PersonNumber)
            {
                return NotFound();
            }
            var existing  = await _context.Member.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            EditableProperties(member, existing);
            if (!ModelState.IsValid) return View(member);
            try
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(member.PersonNumber))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private static void EditableProperties(Member from, Member to)
        {
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Membership = from.Membership;
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
