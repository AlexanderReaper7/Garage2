namespace Garage2.Models
{
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

    public class ParkingLotManager : IParkingLotManager
    {
        /// <summary>
        /// stores the id of ParkedVehicle´s with up to 3 Id´s per slot 
        /// </summary>
        private int[,] parkingLot = new int[50, 3];

        public ParkingLotManager(Data.Garage2Context context)
        {
            foreach (var vehicle in context.ParkedVehicle)
            {
                var i = vehicle.ParkingSpace;
                i -= 1;
                var j = vehicle.ParkingSubSpace;
                Park((i, j), vehicle.VehicleType.GetVehicleSize(), vehicle.Id);
            }
        }

        public int LargestParkingSpaceAvailable
        {
            get
            {
                int largest = 0; // Keeps track of the largest available space found
                int counter = 0; // Keeps track of the current count of consecutive empty slots

                for (int i = 0; i < parkingLot.GetLength(0); i++)
                {
                    for (int j = 0; j < parkingLot.GetLength(1); j++)
                    {
                        if (parkingLot[i, j] == 0)
                        {
                            // If the current slot is empty, increment the counter
                            counter++;
                            // Check if the largest needs to be updated
                            if (largest < counter) largest = counter;
                        }
                        else
                        {
                            // If the current slot is not empty, reset the counter
                            counter = 0;
                            // Breaks the loop so you want be able to park at for example a half parking space
                            if (largest >= parkingLot.GetLength(1)) break;
                        }
                    }
                }
                return largest; // Return the size of the largest available space
            }
        }

        /// <summary>
        /// parks a vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subSlotSize">the number of partial spaces in the parking lot ex. 3 for a car, 1 for a motorcycle and 9 for boats and airplanes</param>
        /// <exception cref="Exception"></exception>
        /// <returns>the numbered parking lot position in (slot, sub-slot) format</returns>
        public (int, int) AddVehicleToSlot(int id, int subSlotSize)
        {
            var currentSize = 0;

            for (int i = 0; i < parkingLot.GetLength(0); i++)
            {
                for (int j = 0; j < parkingLot.GetLength(1); j++)
                {
                    if (parkingLot[i, j] == 0)
                    {
                        // Count slot
                        currentSize += 1;
                    }
                    else
                    {
                        // Reset counter
                        currentSize = 0;
                        // if the subSlotSize is smaller than the remaining possible sub-slots in this parking slot then contine checking
                        if (parkingLot.GetLength(1) < subSlotSize) continue;
                        // Else, check next parkingSlot
                        break;
                    }
                    // If the vehicle fits here, then park
                    if (currentSize == subSlotSize)
                    {
                        // Park here by adding the vehicle Id to each slot, signifying that they´re taken
                        Park((i, j), subSlotSize, id);
                        return (i + 1, j);
                    }
                }
            }
            throw new Exception("Couldn't find anywhere to park.");
        }

        /// <summary>
        /// fills the following parking slots with the database id
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="slotSize"></param>
        private void Park((int, int) startIndex, int slotSize, int vehicleId)
        {
            // deconstruct index into separate vars
            var (first, second) = startIndex;
            var count = 0;
            for (int i = first; i < parkingLot.GetLength(0) && count < slotSize; i++)
            {
                for (int j = second; j < parkingLot.GetLength(1) && count < slotSize; j++)
                {
                    count++;
                    parkingLot[i, j] = vehicleId;
                }
            }
        }


        /// <summary>
        /// un-parks a vehicle
        /// </summary>
        /// <param name="id">the database id of the vehicle</param>
        /// <param name="parkingSpace">starting location of the space in ParkingLot</param>
        public void RemoveVehicleFromLot(int id, (int, int) parkingSpace, int slotSize)
        {
            var count = 0;
            var (index, subIndex) = parkingSpace;
            index -= 1;
            for (int i = index; i < parkingLot.GetLength(0) && count < slotSize; i++)
            {
                for (int j = subIndex; j < parkingLot.GetLength(1) && count < slotSize; j++)
                {
                    count++;
                    if (parkingLot[i,j] == id)
                    {
                        parkingLot[i,j] = 0;
                    }
                    else
                    {
                        throw new Exception("encountered an id not belonging to the parked vehicle");
                    }
                }
            }
        }
    }
}
