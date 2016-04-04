using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		double baseLivingRoomArea = 24 * 12;

		// Ugly hacks for my lists from GH
		int[] DefaultLivingRoomAreas = { 500, 650, 800, 900 };
		int[] DimensionsForKitchens = { 8, 12, 16 };
		string[] LvTypes = { "Main", "Dining", "Reception", "Library", "Other" };

		#region Kitchen Data
		double singleKitchenArea;
		int numOfKitchens;

		public double SingleKitchenArea => singleKitchenArea;
		public double TotalKitchenArea => singleKitchenArea * numOfKitchens;
		public int NumberOfKitchens => numOfKitchens;
		#endregion

		#region Living Room data
		double totalLivingArea;
		int numberOfLivingRooms;

		public double TotalLivingArea => totalLivingArea;
		public int NumberOfLivingRooms => numberOfLivingRooms;
		public double SingleLivingRoomArea => totalLivingArea / numberOfLivingRooms;

		// public string[] LivingRoomTypes = { "Main", "Dining", "Reception", "Library", "Other" };
		// This list seems contrived. Do I even need it? Commented out for now
		#endregion

		#region Bedroom Data
		double typicalBedroomsArea = 0;
		int numberOfTypicalBedrooms = 0;

		double oddBedroomsArea = 0;
		int numberOfOddBedrooms = 0;

		double couplesRoomsArea = 0;
		int coupleRoomCount = 0;

		public double TypicalBedroomsArea => typicalBedroomsArea;
		public int NumberOfTypicalBedrooms => numberOfTypicalBedrooms;

		public double OddBedroomArea => oddBedroomsArea;
		public int NumberOfOddBedrooms => numberOfOddBedrooms; // Either one or two odd bedrooms. one for boys and one for girls.

		public double CouplesRoomsArea => couplesRoomsArea;
		public double NumberOfCouplesRooms => coupleRoomCount;

		public double TotalBedroomsArea => (typicalBedroomsArea + oddBedroomsArea + couplesRoomsArea);
		public int TotalNumberOfBedrooms => (numberOfTypicalBedrooms + numberOfOddBedrooms + coupleRoomCount);
		#endregion


		// Constructor and main calulcations
		public BldgProgram(Input input)
		{
			sons = input.Sons;
			dtrs = input.Daughters;
			parents = input.Parents;
			gparents = input.GParents;
			kpbr = input.KidsPerBedroom;
			plotDepth = input.PlotDepth;
			plotWidth = input.PlotWidth;
			totalResidents = input.Total;

			// Doesnt account for direction
			Database.Boundary = new Rectangle(plotWidth / 4, plotDepth / 4);

			#region Kitchen Calcs
			var residentFactor = (int)(Ceiling(totalResidents / 2) - 1);
			if(residentFactor < DimensionsForKitchens.Count())
			{
				singleKitchenArea = DimensionsForKitchens[residentFactor] * 12;
				new Room("Kitchen", new Point(), new Rectangle(DimensionsForKitchens[residentFactor] / 4, 12 / 4));
				numOfKitchens = 1;
			}
			else
			{
				singleKitchenArea = DimensionsForKitchens.Last() * 12;
				new Room("Clean Kitchen", new Point(), new Rectangle(DimensionsForKitchens.Last() / 4, 12 / 4));
				new Room("Dirty Kitchen", new Point(), new Rectangle(DimensionsForKitchens.Last() / 4, 12 / 4));
				numOfKitchens = 2;
			}
			#endregion

			#region Living Room Calcs
			// There must be some better way to do the math
			if(totalResidents <= 4)
			{
				residentFactor = (int)(totalResidents - 1);
				totalLivingArea = DefaultLivingRoomAreas[residentFactor] - TotalKitchenArea;
			}
			else
			{
				residentFactor = (int)((totalResidents - 4) * 100);
				totalLivingArea = residentFactor + 900 - TotalKitchenArea;
			}

			// These calcs are such a hack ........... wtf
			numberOfLivingRooms = (int)Ceiling(totalLivingArea / baseLivingRoomArea);
			var potentialLivingRoomArea = totalLivingArea / numberOfLivingRooms;
			var otherLivingRoomDimension = potentialLivingRoomArea / 12;
			var dimensionRoundedToGrid = (int)Ceiling(otherLivingRoomDimension / 4);

			for(int i = 0; i < numberOfLivingRooms; i++)
			{
				var name = i < LvTypes.Length ? LvTypes[i] : LvTypes.Last();
				new Room(name, new Point(), new Rectangle(dimensionRoundedToGrid, 3));
				// How the fuck will I do the pairings? Do I need multiple lists of Rooms?
			}
			#endregion

			#region Bedroom Calcs
			CalcKidsBedrooms(sons);
			CalcKidsBedrooms(dtrs);

			// Parents Bedroom Calcs

			if(parents > 0)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				new Room("Parents Bedroom", new Point(), new Rectangle(5, 3));
			}

			// Doesnt take into account that maybe the 2 Grandparents arent a couple !!
			if(gparents > 0)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				new Room("Grandparents Bedroom", new Point(), new Rectangle(5, 3));
			}

			if(gparents > 2)
			{
				coupleRoomCount++;
				couplesRoomsArea += 12 * 20;
				new Room("Second Grandparents Bedroom", new Point(), new Rectangle(5, 3));
			}

			if(TotalNumberOfBedrooms >= 5)
				new Room("Corridor", new Point(), new Rectangle(3 * TotalNumberOfBedrooms, 1));
			// This is so bad

			#endregion
		}

		void CalcKidsBedrooms(int kids)
		{
			double NumberOfTypicalBrs = Floor((double)kpbr / kids);
			int residentFactor = (sons % kpbr);

			numberOfTypicalBedrooms += (int)NumberOfTypicalBrs;

			if(residentFactor != 0)
			{
				if(residentFactor <= 2)
				{
					oddBedroomsArea += 16 * 12;
					numberOfOddBedrooms++;
					new Room("Older Kids Bedroom", new Point(), new Rectangle(4, 3));
				}
				else
				{
					oddBedroomsArea += ((4 * (residentFactor - 1)) + 12) * 12;
					numberOfOddBedrooms++;
					new Room("Older Kids Bedroom", new Point(), new Rectangle(((residentFactor - 1)) + 3, 3));
				}
			}

			typicalBedroomsArea += (((4 * kpbr) + 8) * 12) * NumberOfTypicalBrs;

			for(int i = 0; i < NumberOfTypicalBrs; i++)
				new Room("Kids Bedroom", new Point(), new Rectangle(kpbr + 2, 3));

		}
	}
}
