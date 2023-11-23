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
using Garage2.Services;


namespace Garage2.Controllers;


public class ParkedVehiclesController : Controller
{
    private readonly IParkingLotManager parkingLotManager;
    private readonly Garage2Context context;
    private readonly VehicleStatistics vehicleStatistics;
    private readonly AvailableParkingLotsViewModel availableParkingLotsViewModel;
    private readonly IMapper mapper;
    private readonly IMessageToView messageToView;
    public ParkedVehiclesController(Garage2Context context, IParkingLotManager parkingLotManager, IMapper mapper, IMessageToView messageToView)
    {
        this.parkingLotManager = parkingLotManager;
        this.context = context;
        vehicleStatistics = new VehicleStatistics();
        availableParkingLotsViewModel = new AvailableParkingLotsViewModel();
        this.mapper = mapper;
        this.messageToView = messageToView;
    }

    // GET: ParkedVehicles
    public async Task<IActionResult> Index(string? sortOrder = null, string? owner = null, bool? isParked = null, string[]? vehicleTypes = null,
        string? color = null, string? brand = null, string? model = null, string? regNum = null)
    {
        // Get all parked vehicles
        var vehicles = context.ParkedVehicle.Include(v => v.VehicleType).Include(m => m.Member).AsQueryable();
        // Apply filters
        vehicles = ApplyFilters(vehicles, regNum, owner, isParked, vehicleTypes, color, brand, model);

        // Sort
        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "RegistrationNumber_asc";
        }
        ViewBag.CurrentSort = sortOrder;
        vehicles = sortOrder switch
        {
            "RegistrationNumber_asc" => vehicles.OrderBy(v => v.RegistrationNumber),
            "RegistrationNumber_desc" => vehicles.OrderByDescending(v => v.RegistrationNumber),
            "Owner_asc" => vehicles.OrderBy(v => v.Member.FirstName + v.Member.LastName),
            "Owner_desc" => vehicles.OrderByDescending(v => v.Member.FirstName + v.Member.LastName),
            "VehicleType_asc" => vehicles.OrderBy(v => v.VehicleType),
            "VehicleType_desc" => vehicles.OrderByDescending(v => v.VehicleType),
            "Brand_asc" => vehicles.OrderBy(v => v.Brand),
            "Brand_desc" => vehicles.OrderByDescending(v => v.Brand),
            "Model_asc" => vehicles.OrderBy(v => v.Model),
            "Model_desc" => vehicles.OrderByDescending(v => v.Model),
            "Color_asc" => vehicles.OrderBy(v => v.Color),
            "Color_desc" => vehicles.OrderByDescending(v => v.Color),
            "ArrivalTime_asc" => vehicles.OrderBy(v => v.ArrivalTime),
            "ArrivalTime_desc" => vehicles.OrderByDescending(v => v.ArrivalTime),
            "ParkingSpace_asc" => vehicles.OrderBy(v => v.ParkingSpace),
            "ParkingSpace_desc" => vehicles.OrderByDescending(v => v.ParkingSpace),
            _ => vehicles.OrderBy(v => v.RegistrationNumber)
        };

        return View("Index", await vehicles.ToListAsync());
    }

    private static IQueryable<ParkedVehicle> ApplyFilters(IQueryable<ParkedVehicle> vehicles, string? regNum = null,
        string? owner = null, bool? isParked = null, string[]? vehicleTypes = null,
        string? color = null, string? brand = null, string? model = null)
    {
        if (!string.IsNullOrEmpty(regNum))
            vehicles = vehicles.Where(v =>
                v.RegistrationNumber.Contains(regNum));
        if (!string.IsNullOrEmpty(owner))
            vehicles = vehicles.Where(v =>
                v.Member.FirstName.Contains(owner) ||
                v.Member.LastName.Contains(owner));
        if (isParked is { } b)
        {
            vehicles = b ? vehicles.Where(v => v.ParkingSpace != 0) : vehicles.Where(v => v.ParkingSpace == 0);
        }

        if (vehicleTypes is { Length: > 0 })
        {
            foreach (var vehicleType in vehicleTypes)
            {
                vehicles = vehicles.Where(v => v.VehicleType.Name == vehicleType);
            }
        }
        if (!string.IsNullOrEmpty(color))
            vehicles = vehicles.Where(v => v.Color.Contains(color));
        if (!string.IsNullOrEmpty(brand))
            vehicles = vehicles.Where(v => v.Brand.Contains(brand));
        if (!string.IsNullOrEmpty(model))
            vehicles = vehicles.Where(v => v.Model.Contains(model));
        return vehicles;
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
        return View();
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
            // Registering a vehicle doesn't park it so set arrival time to min value
            parkedVehicle.ArrivalTime = DateTime.MinValue;

            context.Add(parkedVehicle);

            await context.SaveChangesAsync();

            messageToView.ShowMessageInView("Parked Info:");
            return View("Details", parkedVehicle);
        }
        return View(parkedVehicle);
    }

    /// <summary>
    /// adds the vehicle to the parking lot and updates the parked vehicle with the parking space and sub-space numbers
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Park(int id)
    {
        ParkedVehicle parkedVehicle;
        try
        {
            parkedVehicle = context.ParkedVehicle.Include(p => p.Member).Include(p => p.VehicleType).First(p => p.Id == id);
        }
        catch (Exception)
        {
            return NotFound();
        }
        if (parkedVehicle.ParkingSpace != 0) return NotFound(); // vehicle is already parked
        var parkingLot = parkingLotManager.Park(parkedVehicle.Id, parkedVehicle.VehicleType.Size);
        parkedVehicle.ParkingSpace = parkingLot.Item1;
        parkedVehicle.ParkingSubSpace = parkingLot.Item2;
        parkedVehicle.ArrivalTime = DateTime.Now;
        await context.SaveChangesAsync();
        return View("Details", parkedVehicle);
    }
    /// <summary>
    /// UnParks the vehicle and returns a receipt
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnPark(int id)
    {
        ParkedVehicle parkedVehicle;
        try
        {
            parkedVehicle = context.ParkedVehicle.Include(p => p.Member).Include(p => p.VehicleType).First(p => p.Id == id);
        }
        catch (Exception)
        {
            return NotFound();
        }
        if (parkedVehicle.ParkingSpace == 0) return NotFound(); // vehicle is not parked
        var checkoutModel = CheckOutDetails(parkedVehicle);
        var space = parkedVehicle.ParkingSpace;
        var subspace = parkedVehicle.ParkingSubSpace;
        parkingLotManager.UnPark(parkedVehicle.Id, (space, subspace), parkedVehicle.VehicleType.Size);
        parkedVehicle.ParkingSpace = 0;
        parkedVehicle.ParkingSubSpace = 0;
        parkedVehicle.ArrivalTime = DateTime.MinValue;
        await context.SaveChangesAsync();
        return View("Receipt", checkoutModel);
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

                throw;
            }


            messageToView.ShowMessageInView("New edited info:");
            return View("Details", parkedVehicle);
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
        var parkedVehicle = await context.ParkedVehicle.Include(m => m.Member)
            .Include(parkedVehicle => parkedVehicle.VehicleType).FirstAsync(m => m.Id == id);
        if (parkedVehicle == null)
        {
            return NotFound();
        }

        var checkOutModel = CheckOutDetails(parkedVehicle); //ToDo: Is there a way to not call this method again since we already have stored the data in Delete method

        context.ParkedVehicle.Remove(parkedVehicle);
        await context.SaveChangesAsync();
        if (parkedVehicle.ParkingSpace != 0) parkingLotManager.UnPark(parkedVehicle.Id, (parkedVehicle.ParkingSpace, parkedVehicle.ParkingSubSpace), parkedVehicle.VehicleType.Size);
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

        return View(vehicleStatistics);
    }

    public IActionResult ShowAvailableLots()
    {

        availableParkingLotsViewModel.AvailableParkingLotsRegularSize = parkingLotManager.GetAvailableRegularSize(3);
        availableParkingLotsViewModel.AvailableParkingLotsMediumSize = parkingLotManager.GetAvailableRegularSize(6);
        availableParkingLotsViewModel.AvailableParkingLotsLargeSize = parkingLotManager.GetAvailableRegularSize(9);

        return View("AvailableLots", availableParkingLotsViewModel);
    }

    private Dictionary<Membership, int> GetTotalMemberships()
    {
        var membershipCounts = context.Member
            .GroupBy(p => p.Membership)
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
}
