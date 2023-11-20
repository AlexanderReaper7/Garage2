using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Garage2.Models;
using Garage2.Models.Entities;


namespace Garage2.Controllers;


public class ParkedVehiclesController : Controller
{
    private readonly IParkingLotManager parkingLotManager;
    private readonly Garage2Context context;
    private readonly VehicleStatistics vehicleStatistics;
    private readonly IMapper mapper;

    public ParkedVehiclesController(Garage2Context context, IParkingLotManager parkingLotManager, IMapper mapper)
    {
        this.parkingLotManager = parkingLotManager;
        this.context = context;
        vehicleStatistics = new VehicleStatistics();
        this.mapper = mapper;
    }

    // GET: ParkedVehicles
    public async Task<IActionResult> Index(string sortOrder)
    {
        var model = from v in context.ParkedVehicle
                    select v;
        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "RegistrationNumber_asc";
        }
        ViewBag.CurrentSort = sortOrder;
        model = sortOrder switch
        {
            "RegistrationNumber_asc" => model.OrderBy(v => v.RegistrationNumber),
            "RegistrationNumber_desc" => model.OrderByDescending(v => v.RegistrationNumber),
            "VehicleType_asc" => model.OrderBy(v => v.VehicleType),
            "VehicleType_desc" => model.OrderByDescending(v => v.VehicleType),
            "ArrivalTime_asc" => model.OrderBy(v => v.ArrivalTime),
            "ArrivalTime_desc" => model.OrderByDescending(v => v.ArrivalTime),
            "ParkingSpace_asc" => model.OrderBy(v => v.ParkingSpace),
            "ParkingSpace_desc" => model.OrderByDescending(v => v.ParkingSpace),
            _ => model.OrderBy(v => v.RegistrationNumber)
        };

        var viewModel = mapper.ProjectTo<ParkedVehiclesViewModel>(context.ParkedVehicle);

        return View("ParkedVehiclesIndex", await viewModel.ToListAsync());
    }

    // GET: ParkedVehicles/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var parkedVehicle = await context.ParkedVehicle.Include(v => v.VehicleType).Include(m => m.Member)
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
    public async Task<IActionResult> Create([Bind("Id,RegistrationNumber,VehicleType,Color,Brand,Model,NumberOfWheels,ArrivalTime,ParkingSpace,ParkingSubSpace")] ParkedVehicle parkedVehicle)
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

            Park(parkedVehicle);

            await context.SaveChangesAsync();

            Garage2Helpers.Garage2Helpers.MessageToUser = "Parked Info";
            return View("ShowParkedInfo", parkedVehicle);
        }
        return View(parkedVehicle);
    }

    /// <summary>
    /// adds the vehicle to the parking lot and updates the parked vehicle with the parking space and sub-space numbers
    /// </summary>
    /// <param name="parkedVehicle"></param>
    private void Park(ParkedVehicle parkedVehicle)
    {
        var parkingLot = parkingLotManager.AddVehicleToSlot(parkedVehicle.Id, (int)parkedVehicle.VehicleType.Size);
        parkedVehicle.ParkingSpace = parkingLot.Item1;
        parkedVehicle.ParkingSubSpace = parkingLot.Item2;
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,VehicleType,Color,Brand,Model,NumberOfWheels,ArrivalTime,ParkingSpace,ParkingSubSpace")] ParkedVehicle parkedVehicle)
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
                parkedVehicle.ParkingSpace = existingParkedVehicle.ParkingSpace;
                parkedVehicle.ParkingSubSpace = existingParkedVehicle.ParkingSubSpace;
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


            Garage2Helpers.Garage2Helpers.MessageToUser = "Edit Info";
            return View("ShowParkedInfo", parkedVehicle);
        }
        return View(parkedVehicle);
    }

    private static void EditableProperties(ParkedVehicle parkedVehicle, ParkedVehicle existingParkedVehicle)
    {
        existingParkedVehicle.Brand = parkedVehicle.Brand;
        existingParkedVehicle.Color = parkedVehicle.Color;
        existingParkedVehicle.Model = parkedVehicle.Model;
        existingParkedVehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;
    }

    // GET: ParkedVehicles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var parkedVehicle = await context.ParkedVehicle
            .Include(m => m.Member).FirstOrDefaultAsync(m => m.Id == id);

        if (parkedVehicle == null)
        {
            return NotFound();
        }

        var viewModel = CheckOutDetails(parkedVehicle);

        return View(viewModel);
    }

    private CheckOutVehicleViewModel CheckOutDetails(ParkedVehicle parkedVehicle)
    {
        var checkOutModel = mapper.Map<CheckOutVehicleViewModel>(parkedVehicle);

        checkOutModel.CheckOutTime = DateTime.Now;
        checkOutModel.TotalTime = checkOutModel.CheckOutTime - checkOutModel.ArrivalTime;

        int totalHours = (int)checkOutModel.TotalTime.TotalHours;
        int totalMin = checkOutModel.TotalTime.Minutes;

        checkOutModel.Price = (10 * totalHours) + (10 * (decimal)totalMin / 60.0m);

        return checkOutModel;
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
        var parkedVehicle = await context.ParkedVehicle.Include(m => m.Member).FirstOrDefaultAsync(m => m.Id == id);
        if (parkedVehicle == null)
        {
            return NotFound();
        }

        context.ParkedVehicle.Remove(parkedVehicle);
        var checkOutModel = CheckOutDetails(parkedVehicle); //ToDo: Is there a way to not call this method again since we already have stored the data in Delete method

        await context.SaveChangesAsync();
        /* TODO: ADD THIS BACK WHEN CONTROLLER IS ADDED
        parkingLotManager.RemoveVehicleFromLot(parkedVehicle.Id, (parkedVehicle.ParkingSpace, parkedVehicle.ParkingSubSpace), parkedVehicle.VehicleType.GetVehicleSize());
        */
        return View("Receipt", checkOutModel);
    }

    private bool ParkedVehicleExists(int id)
    {
        return (context.ParkedVehicle?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<IActionResult> Statistics()
    {
        var parkedVehicle = await context.ParkedVehicle.Include(m => m.Member).ToListAsync();

        vehicleStatistics.NumberOfWheels = context.ParkedVehicle.Select(v => v.NumberOfWheels).Sum();
        vehicleStatistics.Price = CalculateCurrentEarnings(parkedVehicle);

        vehicleStatistics.VehicleCounts = VehicleCount();
        vehicleStatistics.NrOfMembers = parkedVehicle.Select(p => p.Member).Distinct().Count();
        vehicleStatistics.Memberships = GetTotalMemberships();


        return View("ShowStatistics", vehicleStatistics);
    }

    private Dictionary<Membership, int> GetTotalMemberships()
    {
        var membershipCounts = context.ParkedVehicle.Include(t => t.Member)
            .GroupBy(p => p.Member.Membership)
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );
        return membershipCounts;
    }

    public Dictionary<string, int> VehicleCount()
    {
        var vehicleCount = context.ParkedVehicle
            .GroupBy(p => p.VehicleType.Name)
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );

        return vehicleCount;
    }

    /// <summary>
    /// GET: ParkedVehicles/Search/"searchString"
    /// Search for parked vehicles by registration number
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns></returns>
    public async Task<IActionResult> Search(string searchString)
    {

        var model = mapper.ProjectTo<ParkedVehiclesViewModel>(context.ParkedVehicle);

        if (!string.IsNullOrEmpty(searchString))
        {
            model = model.Where(v => v.RegistrationNumber.Replace(" ", "").Contains(searchString.Replace(" ", "")));
        }

        return View("ParkedVehiclesIndex", await model.ToListAsync());
    }
}
