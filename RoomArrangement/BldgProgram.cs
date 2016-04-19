using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace RoomArrangement
{
	public class BldgProgram
	{
		int sons;
		int dtrs;
		int parents;
		int gparents;
		int kpbr;
		int totalResidents;

		public double PrivatePool { get; private set; } = 750; // feet. For 3 people
		public double PublicPool { get; private set; } = 0;

		public double BedroomPool { get; private set; } = 0;
		public int BedroomCount { get; private set; } = 0;
		public IList<int> NumberOfKidsInTheOlderKidsBedroom { get; private set; } = new List<int>();

		public double KitchenPool { get; private set; } = 0;
		public bool Kitchenette { get; private set; } = false; // if this is true it should expand the Living Room.


		public BldgProgram(Input input)
		{
			sons = input.Sons;
			dtrs = input.Daughters;
			parents = input.Parents;
			gparents = input.Grandparents;
			kpbr = input.KidsPerBedroom;
			totalResidents = input.Total;

			for(int i = totalResidents; i > 3; i--)
				PrivatePool += 100; // feet, for each extra person over 3.

			var inputRooms = input.Rooms;

			CalcKitchens(inputRooms);

			if(inputRooms.HasFlag(InputRooms.Reception))
				if(inputRooms.HasFlag(InputRooms.DirtyKitchen))
					PublicPool += 24 * 16;
				else
					PublicPool += 24 * 12;

			CalcKidsBedrooms(sons);

			CalcKidsBedrooms(dtrs);

			CalcCouplesBedrooms();
		}

		void CalcKitchens(InputRooms inputRooms)
		{
			int[] dimsForKitchens = { 8, 12, 16 };

			var resFactor = (int)(Ceiling(totalResidents / 2d) - 1);
			var dirtyWanted = inputRooms.HasFlag(InputRooms.DirtyKitchen);
			var cleanWanted = inputRooms.HasFlag(InputRooms.CleanKitchen);

			if(dirtyWanted && cleanWanted)
				if(resFactor < dimsForKitchens.Count())
				{
					KitchenPool += dimsForKitchens[resFactor] * 12; // the Dirty Kitchen
					Kitchenette = true; // the clean kitchen
				}
				else
					KitchenPool += dimsForKitchens[resFactor] * 12 * 2; // Both kitchens

			else if(dirtyWanted ^ cleanWanted) // XOR operator FTW
				KitchenPool += dimsForKitchens[resFactor] * 12; // either kitchen
			else
				Kitchenette = true;
		}

		void CalcKidsBedrooms(int kids)
		{
			double numOfTypicalBrs = Floor((double)kpbr / kids);
			int residentFactor = (sons % kpbr);
			int minRoomWidth = 12;

			if(residentFactor != 0)
				if(residentFactor <= 2) // Odd rooms.
				{
					BedroomPool += 16 * minRoomWidth;
					BedroomCount++;
				}
				else
				{
					BedroomPool += ((4 * (residentFactor - 1)) + 12) * minRoomWidth;
					BedroomCount++;
				}

			NumberOfKidsInTheOlderKidsBedroom.Add(residentFactor);
			BedroomPool += (((4 * kpbr) + 8) * minRoomWidth) * numOfTypicalBrs;
			BedroomCount += (int)numOfTypicalBrs;
		}

		void CalcCouplesBedrooms()
		{
			int minRoomWidth = 12;

			if(parents > 0)
			{
				BedroomPool += minRoomWidth * 20;
				BedroomCount++;
			}

			// Doesnt take into account that maybe the 2 Grandparents arent a couple !!
			if(gparents > 0)
			{
				BedroomPool += minRoomWidth * 20;
				BedroomCount++;
			}

			if(gparents > 2)
			{
				BedroomPool += minRoomWidth * 20;
				BedroomCount++;
			}
		}
	}
}
