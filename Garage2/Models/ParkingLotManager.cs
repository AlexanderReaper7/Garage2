﻿using Garage2.Data;
using Garage2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Models
{
    public class ParkingLotManager : IParkingLotManager
    {
        /// <summary>
        /// stores the id of ParkedVehicle´s with up to 3 Id´s per slot 
        /// </summary>
        private int[,] parkingLot = new int[IParkingLotManager.ParkingLotSize, IParkingLotManager.ParkingSubLotSize];

        public ParkingLotManager(Garage2Context context)
        {

            foreach (var vehicle in context.ParkedVehicle.Include(v => v.VehicleType))
            {
                var i = vehicle.ParkingSpace;
                if (i == 0) continue; // Skip vehicles that are not parked
                i -= 1;
                var j = vehicle.ParkingSubSpace;
                Park((i, j), vehicle.VehicleType.Size, vehicle.Id);
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
        /// <param name="size">the number of partial spaces in the parking lot ex. 3 for a car, 1 for a motorcycle and 9 for boats and airplanes</param>
        /// <exception cref="Exception"></exception>
        /// <returns>the numbered parking lot position in (slot, sub-slot) format</returns>
        public (int, int) Park(int id, int size)
        {
            for (int i = 0; i < parkingLot.GetLength(0); i++)
            {
                for (int j = 0; j < parkingLot.GetLength(1); j++)
                {
                    if (parkingLot[i, j] == 0)
                    {
                        // If the current slot is empty, try to park the vehicle starting at this position
                        if (CanParkAt(i, j, size))
                        {
                            Park((i, j), size, id);
                            return (i + 1, j);
                        }
                    }
                }
            }

            throw new Exception("Couldn't find anywhere to park.");
        }

        /// <summary>
        /// Checks if the vehicle can be parked at the given position
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool CanParkAt(int row, int col, int size)
        {
            var count = 0;
            for (int i = row; i < parkingLot.GetLength(0); i++)
            {
                for (int j = col; j < parkingLot.GetLength(1); j++)
                {
                    count++;
                    if (count == size)
                    {
                        // If the count is equal to the size of the vehicle, return true
                        return true;
                    }

                    if (parkingLot[i, j] != 0)
                    {
                        return false; // Vehicle can't be parked here as some slots are already occupied
                    }
                }
            }

            return false; // Vehicle can't be parked here as there are not enough slots left
        }

        /// <summary>
        /// fills the following parking slots with the database id
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="size"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Park((int, int) startIndex, int size, int vehicleId)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "size must be greater than 0");
            }

            if (vehicleId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(vehicleId), "vehicleId must be greater than 0");
            }

            // deconstruct index into separate vars
            var (first, second) = startIndex;

            if (first < 0 || second < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                    "the startIndex must be in bounds of the ParkingLot array");
            }

            var count = 0;
            for (int i = first; i < parkingLot.GetLength(0) && count < size; i++)
            {
                for (int j = second; j < parkingLot.GetLength(1) && count < size; j++)
                {
                    count++;
                    parkingLot[i, j] = vehicleId;
                }
            }

            if (count != size)
            {
                throw new Exception("the startIndex and size must be in bounds of the ParkingLot array");
            }
        }


        /// <summary>
        /// un-parks a vehicle
        /// </summary>
        /// <param name="id">the database id of the vehicle</param>
        /// <param name="parkingSpace">starting location of the space in ParkingLot</param>
        /// <param name="size">the number of partial spaces in the parking lot ex. 3 for a car, 1 for a motorcycle and 9 for boats and airplanes</param>
        /// <exception cref="Exception"></exception>
        /// ">
        public void UnPark(int id, (int, int) parkingSpace, int size)
        {
            var count = 0;
            var (index, subIndex) = parkingSpace;
            index -= 1;
            for (int i = index; i < parkingLot.GetLength(0) && count < size; i++)
            {
                for (int j = subIndex; j < parkingLot.GetLength(1) && count < size; j++)
                {
                    count++;
                    if (parkingLot[i, j] == id)
                    {
                        parkingLot[i, j] = 0;
                    }
                    else
                    {
                        throw new Exception("Encountered an id not belonging to the parked vehicle");
                    }
                }
            }
        }

        public int GetAvailableRegularSize(int size)
        {
            int count = 0;

            for (int i = 0; i < parkingLot.GetLength(0); i++)
            {
                for (int j = 0; j < parkingLot.GetLength(1); j++)
                {
                    if (parkingLot[i, j] == 0)
                    {

                        count++;
                    }

                }
            }

            count /= size;

            return count;
        }

    }
}