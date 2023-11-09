namespace Garage2.Models;

public interface IParkingLotManager
{
    int LargestParkingSpaceAvailable { get; }

    /// <summary>
    /// parks a vehicle
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subSlotSize">the number of partial spaces in the parking lot ex. 3 for a car, 1 for a motorcycle and 9 for boats and airplanes</param>
    /// <exception cref="Exception"></exception>
    /// <returns>the numbered parking lot position in (slot, sub-slot) format</returns>
    (int, int) AddVehicleToSlot(int id, int subSlotSize);

    /// <summary>
    /// un-parks a vehicle
    /// </summary>
    /// <param name="id">the database id of the vehicle</param>
    /// <param name="parkingSpace">starting location of the space in ParkingLot</param>
    void RemoveVehicleFromLot(int id, (int, int) parkingSpace, int slotSize);
}