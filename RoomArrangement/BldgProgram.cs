using System.Linq;
using static System.Math;
using System.Collections;
using System.Collections.Generic;

namespace RoomArrangement
{
	class BldgProgram
	{
		int sons;
		int dtrs;
		int parents;
		int gparents;
		int kpbr;
		double totalResidents;

		// Ugly hacks for my criteria. TODO to replace with a proper structure once I figure out the criteria.
		// Criteria should also include stuff like preferable Room dimensions
		double baseLivingRoomArea = 24 * 12;
		int[] DefaultLivingRoomAreas = { 500, 650, 800, 900 };
		int[] DimsForKitchens = { 8, 12, 16 };

		// Kitchen Data
		public double KitchenArea { get; private set; }
		public Rectangle KitchenSpace { get; private set; }
		public int KitchensCount { get; private set; }
		public double KitchensTotalArea => KitchenArea * KitchensCount;

		// Living Room Data
		public double LivingRoomArea => LivingRoomsTotalArea / LivingRoomsCount;
		public Rectangle LivingRoomSpace { get; private set; }
		public int LivingRoomsCount { get; private set; }
		public double LivingRoomsTotalArea { get; private set; }

		// Bedroom Data
		public List<double> BedroomTypicalAreas { get; private set; } = new List<double>();
		public List<Rectangle> BedroomTypSpaces { get; private set; } = new List<Rectangle>();
		public int BedroomTypCount => BedroomTypSpaces.Count;

		public List<double> BedroomOddAreas { get; private set; } = new List<double>();
		public List<Rectangle> BedroomOddSpaces { get; private set; } = new List<Rectangle>();
		public int BedroomOddCount => BedroomOddSpaces.Count;
		// Either one or two odd bedrooms. one for boys and one for girls.

		public List<double> BedroomCouplesAreas { get; private set; } = new List<double>();
		public List<Rectangle> BedroomCouplesSpaces { get; private set; } = new List<Rectangle>();

		public int BedroomCouplesCount => BedroomCouplesSpaces.Count;


		public double TotalBedroomsArea => (BedroomTypicalAreas.Sum() + BedroomOddAreas.Sum() + BedroomCouplesAreas.Sum());
		public int TotalNumberOfBedrooms => (BedroomTypCount + BedroomOddCount + BedroomCouplesCount);

		// Constructor and main calulcations
		public BldgProgram(Input input)
		{
			sons = input.Sons;
			dtrs = input.Daughters;
			parents = input.Parents;
			gparents = input.GParents;
			kpbr = input.KidsPerBedroom;
			totalResidents = input.Total;

			CalcKitchensAndLivingRooms();

			CalcKidsBedrooms(sons);

			CalcKidsBedrooms(dtrs);

			CalcCouplesBedrooms();

			// Set adjacencies.
			// Adjacency Schedule Mockup
			//
			//		Bedroom		Kitchen		LivingRoom
			// LivingRoom	Aye		Aye		-
			// Kitchen	Nay		-	
			// Bedroom	-
			// 
			// So any bedroom connected to any Living room?
			// I need Living Room Types !!
			//
			// Same for kitchens.
		}

		void CalcKitchensAndLivingRooms()
		{
			// Kitchen Calcs
			var resFactor = (int)(Ceiling(totalResidents / 2) - 1);

			KitchenSpace = new Rectangle(DimsForKitchens[resFactor] / 4, 3);
			if(resFactor < DimsForKitchens.Count())
			{
				KitchenArea = DimsForKitchens[resFactor] * 12;
				KitchensCount = 1;
			}
			else
			{
				KitchenArea = DimsForKitchens.Last() * 12;
				KitchensCount = 2;
			}

			// Living Rooms areas dependant on Kitchens.
			// There must be some better way to do the math
			if(totalResidents <= 4)
			{
				resFactor = (int)(totalResidents - 1);
				LivingRoomsTotalArea = DefaultLivingRoomAreas[resFactor] - KitchensTotalArea;
			}
			else
			{
				resFactor = (int)((totalResidents - 4) * 100);
				LivingRoomsTotalArea = resFactor + 900 - KitchensTotalArea;
			}

			// These calcs are such a hack ........... wtf
			// However the intent is somewhat readable this way
			LivingRoomsCount = (int)Ceiling(LivingRoomsTotalArea / baseLivingRoomArea);

			var actualArea = LivingRoomsTotalArea / LivingRoomsCount;
			var otherLRDim = actualArea / 12;
			var dimRoundedToGrid = (int)Ceiling(otherLRDim / 4);

			LivingRoomSpace = new Rectangle(dimRoundedToGrid, 3);
		}

		void CalcKidsBedrooms(int kids)
		{
			double numOfTypicalBrs = Floor((double)kpbr / kids);
			int residentFactor = (sons % kpbr);

			if(residentFactor != 0)
			{
				if(residentFactor <= 2)
				{
					BedroomOddAreas.Add(16 * 12);
					BedroomOddSpaces.Add(new Rectangle(4, 3));
				}
				else
				{
					BedroomOddAreas.Add(((4 * (residentFactor - 1)) + 12) * 12);
					BedroomOddSpaces.Add(new Rectangle(residentFactor + 2, 3));
				}
			}

			BedroomTypicalAreas.Add((((4 * kpbr) + 8) * 12) * numOfTypicalBrs);

			for(int i = 0; i < numOfTypicalBrs; i++)
				BedroomTypSpaces.Add(new Rectangle(kpbr + 2, 3));
		}

		void CalcCouplesBedrooms()
		{
			if(parents > 0)
			{
				BedroomCouplesAreas.Add(12 * 20);
				BedroomCouplesSpaces.Add(new Rectangle(3, 5));
			}

			// Doesnt take into account that maybe the 2 Grandparents arent a couple !!
			if(gparents > 0)
			{
				BedroomCouplesAreas.Add(12 * 20);
				BedroomCouplesSpaces.Add(new Rectangle(3, 5));
			}

			if(gparents > 2)
			{
				BedroomCouplesAreas.Add(12 * 20);
				BedroomCouplesSpaces.Add(new Rectangle(3, 5));
			}
		}
	}
}