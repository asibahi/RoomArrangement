namespace RoomArrangement
{
	public class Root : LivingRoom
	{
		readonly int mainRoomID;
		override public string Name => $"Main {mainRoomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int MainRoomCount { get; set; }

		protected override bool Flexible => false;

		public Root(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			mainRoomID = ++MainRoomCount;
		}
	}
}
