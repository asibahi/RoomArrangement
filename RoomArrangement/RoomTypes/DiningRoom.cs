namespace RoomArrangement
{
	public class DiningRoom : LivingRoom
	{
		int DiningRoomID { get; }
		override public string Name => $"Dining{DiningRoomID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int DiningRoomCount { get; set; }

		protected override bool Flexible => false;

		public DiningRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			DiningRoomID = ++DiningRoomCount;
		}
	}
}
