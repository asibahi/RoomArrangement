namespace RoomArrangement
{
	class Kitchen : Room
	{
		readonly int kitchenID;
		override public string Name => $"Kitchen {kitchenID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int KitchenCount { get; set; }

		public Kitchen(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			kitchenID = ++KitchenCount;
		}

		public Kitchen(int x, int y) : base(x, y) { }
	}
}
