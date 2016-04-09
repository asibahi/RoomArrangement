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

		public LivingRoom(int x, int y) : base(x, y) { }
	}
}
