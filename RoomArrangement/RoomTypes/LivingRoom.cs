namespace RoomArrangement
{
	public class LivingRoom : Room
	{
		readonly int livingRoomID;
		override public string Name => $"Living{livingRoomID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int LivingRoomCount { get; set; }

		protected override bool Flexible => true;

		public LivingRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			livingRoomID = ++LivingRoomCount;
		}
	}
}
