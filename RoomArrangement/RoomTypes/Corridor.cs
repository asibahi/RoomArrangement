using System;

namespace RoomArrangement
{
	public class Corridor : Room
	{
		readonly int corridorID;
		override public string Name => $"Corridor {corridorID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int CorridorCount { get; set; }

		protected override bool Flexible => true;

		public Corridor(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			corridorID = ++CorridorCount;
		}
	}
}