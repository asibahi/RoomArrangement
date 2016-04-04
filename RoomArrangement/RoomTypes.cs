namespace RoomArrangement
{
	class LivingRoom : Room
	{
		readonly int livingRoomID;
		override public string Name => $"Living {livingRoomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int LivingRoomCount { get; set; }

		public LivingRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			livingRoomID = ++LivingRoomCount;
		}
	}

	class Kitchen : Room
	{
		readonly int kitchenID;
		override public string Name => $"Kitchen {kitchenID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int KitchenCount { get; set; }

		public Kitchen(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			kitchenID = ++KitchenCount;
		}
	}

	class Bedroom : Room
	{
		readonly int bedroomID;
		override public string Name => $"Bedroom {bedroomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int BedroomCount { get; set; }

		public Bedroom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			bedroomID = ++BedroomCount;
		}
	}

	class Corridor : Room
	{
		readonly int corridorID;
		override public string Name => $"Corridor {corridorID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int CorridorCount { get; set; }

		public Corridor(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			corridorID = ++CorridorCount;
			Database.CorridorExists = true;
		}
	}
}