using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models;
using Garage2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Garage2.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly Garage2Context _context;
        private readonly CheckOutVehicleViewModel checkOutModel;

        public ParkedVehiclesController(Garage2Context context)
        {
            _context = context;
            checkOutModel = new CheckOutVehicleViewModel();
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            var model = _context.ParkedVehicle.Select(v => new ParkedVehiclesViewModel
            {
                Id = v.Id,
                RegistrationNumber = v.RegistrationNumber,
                VehicleType = v.VehicleType,
                ArrivalTime = v.ArrivalTime
            });

            return View("ParkedVehiclesIndex", await model.ToListAsync());
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create()
        {
            var model = new ParkedVehicle
            {
                ArrivalTime = DateTime.Now,
            };

            return View(model);
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegistrationNumber,VehicleType,Color,Brand,Model,NumberOfWheels,ArrivalTime")] ParkedVehicle parkedVehicle)
        {
            // normalize the registration number
            string[] parts = parkedVehicle.RegistrationNumber.ToUpperInvariant().Split(' ');
            if (parts.Length == 1)
            {
                // if there is no explicit nationality then assume it is Swedish
                parts = new string[] { "SE", parts[0] };
            }
            parkedVehicle.RegistrationNumber = $"{parts[0]} {parts[1]}";
            ModelState.SetModelValue("RegistrationNumber", new ValueProviderResult(parkedVehicle.RegistrationNumber));

            if (ModelState.IsValid)
            {
                // Check for duplicate registration number
                if (_context.ParkedVehicle.Any(v => v.RegistrationNumber == parkedVehicle.RegistrationNumber))
                {
                    ModelState.AddModelError("RegistrationNumber", "Registration number already exists");
                    return View(parkedVehicle);
                }
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                return View("ShowParkedInfo", parkedVehicle);
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)

        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,VehicleType,Color,Brand,Model,NumberOfWheels,ArrivalTime")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve existing vehicle in database
                    var existingParkedVehicle = await _context.ParkedVehicle.FindAsync(id);
                    if (existingParkedVehicle == null)
                    {
                        return NotFound();
                    }
                    // Only edit the properties we want the user to be able to edit
                    EditableProperties(parkedVehicle, existingParkedVehicle);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
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
            return View(parkedVehicle);
        }

        private static void EditableProperties(ParkedVehicle parkedVehicle, ParkedVehicle existingParkedVehicle)
        {
            existingParkedVehicle.Brand = parkedVehicle.Brand;
            existingParkedVehicle.Color = parkedVehicle.Color;
            existingParkedVehicle.Model = parkedVehicle.Model;
            existingParkedVehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;
            existingParkedVehicle.VehicleType = parkedVehicle.VehicleType;
        }

        // GET: ParkedVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parkedVehicle == null)
            {
                return NotFound();
            }

            CheckOutDetails(parkedVehicle);

            return View(checkOutModel);
        }

        private void CheckOutDetails(ParkedVehicle parkedVehicle)
        {
            checkOutModel.ArrivalTime = parkedVehicle.ArrivalTime;
            checkOutModel.Id = parkedVehicle.Id;
            checkOutModel.RegistrationNumber = parkedVehicle.RegistrationNumber;
            checkOutModel.CheckOutTime = DateTime.Now;
            checkOutModel.TotalTime = checkOutModel.CheckOutTime - checkOutModel.ArrivalTime;

            int totalHours = (int)checkOutModel.TotalTime.TotalHours;
            int totalMin = checkOutModel.TotalTime.Minutes;

            checkOutModel.Price = (10 * totalHours) + (10 * (decimal)totalMin / 60.0m);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            _context.ParkedVehicle.Remove(parkedVehicle);
            CheckOutDetails(parkedVehicle);

            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return View("Receipt", checkOutModel);
        }

        private bool ParkedVehicleExists(int id)
        {
            return (_context.ParkedVehicle?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Statics()
        {
            return View("ShowStatics");
        }
    }
}
