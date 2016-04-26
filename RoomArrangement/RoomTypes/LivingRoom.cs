namespace RoomArrangement
{
	public class LivingRoom : Room
	{
		int LivingRoomID { get; }
		override public string Name => $"Living{LivingRoomID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int LivingRoomCount { get; set; }

		protected override bool Flexible => true;

		public LivingRoom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			LivingRoomID = ++LivingRoomCount;
		}
	}
}
