namespace RoomArrangement
{
	public class Reception : LivingRoom
	{
		int ReceptionID { get; }
		override public string Name => $"Reception{ReceptionID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int ReceptionCount { get; set; }

		protected override bool Flexible => false;

		public Reception(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			ReceptionID = ++ReceptionCount;
		}
	}
}
