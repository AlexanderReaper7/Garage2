namespace Garage2.Models
{
	public class ParkingSlotManager
	{
		private int[] parkingSlots = new int[50];
		private int currentIndex = 0;
		public void AddVehicleToSlot(int id)
		{
			for (int i = currentIndex; i < parkingSlots.Length; i++)
			{
				if (parkingSlots[i] == 0)
				{
					parkingSlots[i] = id;
					break; // Exit the loop after adding to the first available slot

				}
			}
		}

		//public void AddVehicleToSlot(int id)
		//{
		//	for (int i = currentIndex; i < parkingSlots.Length; i++)
		//	{
		//		if (parkingSlots[i] == 0)
		//		{
		//			parkingSlots[i] = id;
		//			currentIndex = i++; // Update the current index for the next call
		//			break; // Exit the loop after adding to the first available slot

		//		}
		//	}
		//}

		public void RemoveVehicleFromSlot(int id)
		{
			for (int i = 0; i < parkingSlots.Length; i++)
			{
				if (parkingSlots[i] == id)
				{
					parkingSlots[i] = 0;
					break;
				}
			}
		}
	}
}
