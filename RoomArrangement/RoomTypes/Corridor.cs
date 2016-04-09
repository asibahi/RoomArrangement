namespace RoomArrangement
{
	class Corridor : Room
	{
		readonly int corridorID;
		override public string Name => $"Corridor {corridorID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int CorridorCount { get; set; }

		public Corridor(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			corridorID = ++CorridorCount;
		}

		public Corridor(int x, int y) : base(x, y) { }
	}
}