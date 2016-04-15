namespace RoomArrangement
{
	class Kitchen : Room, IWet
	{
		readonly int kitchenID;
		override public string Name => $"Kitchen {kitchenID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int KitchenCount { get; set; }

		public Kitchen(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			kitchenID = ++KitchenCount;
		}
	}
}
