using System.Linq;
using static System.Math;

namespace RoomArrangement
{
	class BldgProgram
	{
		int sons;
		int dtrs;
		int parents;
		int gparents;
		int kpbr;
		int plotWidth;
		int plotDepth;
		double totalResidents;
		House house;

		// Ugly hacks for my criteria. TODO to replace with a proper structure once I figure out the criteria.
		// Criteria should also include stuff like preferable Room dimensions
		double baseLivingRoomArea = 24 * 12;
		int[] DefaultLivingRoomAreas = { 500, 650, 800, 900 };
		int[] DimsForKitchens = { 8, 12, 16 };
		string[] LvTypes = { "Main", "Dining", "Reception", "Library", "Other" };

		// Kitchen Data
		double singleKitchenArea;
		int numOfKitchens;
		public double TotalKitchenArea => singleKitchenArea * numOfKitchens;

		// Living Room Data
		double totalLivingArea;
		int numberOfLivingRooms;
		public double SingleLivingRoomArea => totalLivingArea / numberOfLivingRooms;

		// Bedroom Data
		double typicalBedroomsArea = 0;
		int numberOfTypicalBedrooms = 0;

		double oddBedroomsArea = 0;
		int numberOfOddBedrooms = 0; // Either one or two odd bedrooms. one for boys and one for girls.

		double couplesRoomsArea = 0;
		int coupleRoomCount = 0;

		public double TotalBedroomsArea => (typicalBedroomsArea + oddBedroomsArea + couplesRoomsArea);
		public int TotalNumberOfBedrooms => (numberOfTypicalBedrooms + numberOfOddBedrooms + coupleRoomCount);

		// Constructor and main calulcations
		public BldgProgram(Input input, House inputHouse)
		{
			sons = input.Sons;
			dtrs = input.Daughters;
			parents = input.Parents;
			gparents = input.GParents;
			kpbr = input.KidsPerBedroom;
			plotDepth = input.PlotDepth;
			plotWidth = input.PlotWidth;
			totalResidents = input.Total;
			house = inputHouse;

			// Doesnt account for direction
			house.Boundary = new Rectangle(plotWidth / 4, plotDepth / 4);

			CreateKitchensAndLivingRooms();

			CreateKidsBedrooms(sons);
			CreateKidsBedrooms(dtrs);

			CreateCouplesBedrooms();

			if(TotalNumberOfBedrooms >= 5)
				house.AddRoom(RoomType.Corridor, "Bedroom Corridor", Point.Origin, new Rectangle(3 * TotalNumberOfBedrooms, 1));
			// This is so bad
		}

		void CreateKitchensAndLivingRooms()
		{
			// Kitchen Calcs
			var resFactor = (int)(Ceiling(totalResidents / 2) - 1);
			if(resFactor < DimsForKitchens.Count())
			{
				singleKitchenArea = DimsForKitchens[resFactor] * 12;
				house.AddRoom(RoomType.Kitchen, null, Point.Origin, new Rectangle(DimsForKitchens[resFactor] / 4, 12 / 4));
				numOfKitchens = 1;
			}
			else
			{
				singleKitchenArea = DimsForKitchens.Last() * 12;
				house.AddRoom(RoomType.Kitchen, "Clean", Point.Origin, new Rectangle(DimsForKitchens.Last() / 4, 12 / 4));
				house.AddRoom(RoomType.Kitchen, "Dirty", Point.Origin, new Rectangle(DimsForKitchens.Last() / 4, 12 / 4));
				numOfKitchens = 2;
			}

			// Living Rooms areas dependant on Kitchens.
			// There must be some better way to do the math
			if(totalResidents <= 4)
			{
				resFactor = (int)(totalResidents - 1);
				totalLivingArea = DefaultLivingRoomAreas[resFactor] - TotalKitchenArea;
			}
			else
			{
				resFactor = (int)((totalResidents - 4) * 100);
				totalLivingArea = resFactor + 900 - TotalKitchenArea;
			}

			// These calcs are such a hack ........... wtf
			numberOfLivingRooms = (int)Ceiling(totalLivingArea / baseLivingRoomArea);
			var actualArea = totalLivingArea / numberOfLivingRooms;
			var otherLRDim = actualArea / 12;
			var dimRoundedToGrid = (int)Ceiling(otherLRDim / 4);

			for(int i = 0; i < numberOfLivingRooms; i++)
			{
				var name = i < LvTypes.Length ? LvTypes[i] : LvTypes.Last();
				house.AddRoom(RoomType.LivingRoom, name, Point.Origin, new Rectangle(dimRoundedToGrid, 3));
				// How the fuck will I do the pairings? Do I need multiple lists of Rooms?
			}
		}

		void CreateKidsBedrooms(int kids)
		{
			double numOfTypicalBrs = Floor((double)kpbr / kids);
			int residentFactor = (sons % kpbr);

			numberOfTypicalBedrooms += (int)numOfTypicalBrs;

			if(residentFactor != 0)
			{
				if(residentFactor <= 2)
				{
					oddBedroomsArea += 16 * 12;
					numberOfOddBedrooms++;
					house.AddRoom(RoomType.Bedroom, "Older Kids Bedroom", Point.Origin, new Rectangle(4, 3));
				}
				else
				{
					oddBedroomsArea += ((4 * (residentFactor - 1)) + 12) * 12;
					numberOfOddBedrooms++;
					house.AddRoom(RoomType.Bedroom, "Older Kids Bedroom", Point.Origin, new Rectangle(((residentFactor - 1)) + 3, 3));
				}
			}

			typicalBedroomsArea += (((4 * kpbr) + 8) * 12) * numOfTypicalBrs;

			for(int i = 0; i < numOfTypicalBrs; i++)
				house.AddRoom(RoomType.Bedroom, "Kids Bedroom", Point.Origin, new Rectangle(kpbr + 2, 3));
		}

		void CreateCouplesBedrooms()
		{
			if(parents > 0)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				house.AddRoom(RoomType.Bedroom, "Parents Bedroom", Point.Origin, new Rectangle(5, 3));
			}

			// Doesnt take into account that maybe the 2 Grandparents arent a couple !!
			if(gparents > 0)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				house.AddRoom(RoomType.Bedroom, "Grandparents Bedroom", Point.Origin, new Rectangle(5, 3));
			}

			if(gparents > 2)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				house.AddRoom(RoomType.Bedroom, "Second Grandparents Bedroom", new Point(), new Rectangle(5, 3));
			}
		}
	}
}