namespace RoomArrangement
{
	class Reception : LivingRoom
	{
		readonly int receptionID;
		override public string Name => $"Reception {receptionID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int ReceptionCount { get; set; }

		public Reception(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			receptionID = ++ReceptionCount;
		}
	}
}
