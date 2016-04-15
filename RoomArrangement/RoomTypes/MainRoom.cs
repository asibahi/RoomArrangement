namespace RoomArrangement
{
	class MainRoom : LivingRoom
	{
		readonly int mainRoomID;
		override public string Name => $"Main {mainRoomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int MainRoomCount { get; set; }

		public MainRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			mainRoomID = ++MainRoomCount;
		}
	}
}
