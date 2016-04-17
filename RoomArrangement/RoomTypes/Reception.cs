namespace RoomArrangement
{
	public class Reception : LivingRoom
	{
		readonly int receptionID;
		override public string Name => $"Reception {receptionID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int ReceptionCount { get; set; }

		protected override bool Flexible => false;

		public Reception(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			receptionID = ++ReceptionCount;
		}
	}
}
