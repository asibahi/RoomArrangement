namespace RoomArrangement
{
	class DiningRoom : LivingRoom
	{
		readonly int diningRoomID;
		override public string Name => $"Dining {diningRoomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int DiningRoomCount { get; set; }

		public DiningRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			diningRoomID = ++DiningRoomCount;
		}
	}
}
