using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Garage2.Models;

namespace Garage2.Controllers;

public class ParkedVehiclesController : Controller
{
    private readonly IParkingLotManager parkingLotManager;
    private readonly Garage2Context context;
    private readonly CheckOutVehicleViewModel checkOutModel;
    private readonly VehicleStatistics vehicleStatistics;

    public ParkedVehiclesController(Garage2Context context, IParkingLotManager parkingLotManager)
    {
        this.parkingLotManager = parkingLotManager;
        this.context = context;
        checkOutModel = new CheckOutVehicleViewModel();
        vehicleStatistics = new VehicleStatistics();

    }

    // GET: ParkedVehicles
    public async Task<IActionResult> Index()
    {
        var model = context.ParkedVehicle.Select(v => new ParkedVehiclesViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType,
            ArrivalTime = v.ArrivalTime,
            ParkingSpace = v.ParkingSpace,
            ParkingSubSpace = v.ParkingSubSpace
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

        var parkedVehicle = await context.ParkedVehicle
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
            if (context.ParkedVehicle.Any(v => v.RegistrationNumber == parkedVehicle.RegistrationNumber))
            {
                ModelState.AddModelError("RegistrationNumber", "Registration number already exists");
                return View(parkedVehicle);
            }


            context.Add(parkedVehicle);
            await context.SaveChangesAsync();

            parkingLotManager.AddVehicleToSlot(parkedVehicle.Id, parkedVehicle.VehicleType.GetVehicleSize());

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

        var parkedVehicle = await context.ParkedVehicle.FindAsync(id);
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
                var existingParkedVehicle = await context.ParkedVehicle.FindAsync(id);
                if (existingParkedVehicle == null)
                {
                    return NotFound();
                }
                // Only edit the properties we want the user to be able to edit
                EditableProperties(parkedVehicle, existingParkedVehicle);

                await context.SaveChangesAsync();
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

        var parkedVehicle = await context.ParkedVehicle
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

    private decimal CalculateCurrentEarnings(List<ParkedVehicle> parkedVehicle)
    {
        decimal price = 0;

        foreach (var item in parkedVehicle)
        {
            var arrivalTime = item.ArrivalTime;
            var checkOutTime = DateTime.Now;
            var totalTime = checkOutTime - arrivalTime;

            int totalHours = (int)totalTime.TotalHours;
            int totalMinutes = totalTime.Minutes;

            price += (10 * totalHours) + (10 * (decimal)totalMinutes / 60.0m);
        }

        return price;
    }

    // POST: ParkedVehicles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var parkedVehicle = await context.ParkedVehicle.FindAsync(id);
        if (parkedVehicle == null)
        {
            return NotFound();
        }

        context.ParkedVehicle.Remove(parkedVehicle);
        CheckOutDetails(parkedVehicle);

        await context.SaveChangesAsync();

        parkingLotManager.RemoveVehicleFromLot(parkedVehicle.Id, (parkedVehicle.ParkingSpace, parkedVehicle.ParkingSubSpace), parkedVehicle.VehicleType.GetVehicleSize());

        return View("Receipt", checkOutModel);
    }

    private bool ParkedVehicleExists(int id)
    {
        return (context.ParkedVehicle?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<IActionResult> Statistics()
    {
        var parkedVehicle = await context.ParkedVehicle.ToListAsync();

        vehicleStatistics.NumberOfWheels = context.ParkedVehicle.Select(v => v.NumberOfWheels).Sum();
        vehicleStatistics.Price = CalculateCurrentEarnings(parkedVehicle);

        vehicleStatistics.VehicleCounts = VehicleCount();
        //ParkingSlotManager parking = new ParkingSlotManager();
        //parking.AddVehicleToSlot(1);
        //parking.AddVehicleToSlot(2);
        //parking.AddVehicleToSlot(3);

        //parking.RemoveVehicleFromSlot( 2);

        return View("ShowStatistics", vehicleStatistics);
    }

    public Dictionary<VehicleType, int> VehicleCount()
    {
        var vehicleCount = context.ParkedVehicle
            .GroupBy(p => p.VehicleType)
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );

        return vehicleCount;
    }
}