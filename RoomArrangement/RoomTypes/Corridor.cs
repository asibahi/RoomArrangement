namespace RoomArrangement
{
	public class Corridor : Room
	{
		int CorridorID { get; }
		override public string Name => $"Corridor{CorridorID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int CorridorCount { get; set; }

		protected override bool Flexible => true;

		public Corridor(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			CorridorID = ++CorridorCount;
		}
	}
}