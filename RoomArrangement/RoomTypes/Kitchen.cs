namespace RoomArrangement
{
	public class Kitchen : Room, IWet
	{
		readonly int kitchenID;
		override public string Name => $"Kitchen{kitchenID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int KitchenCount { get; set; }

		protected override bool Flexible => false;

		public Kitchen(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			kitchenID = ++KitchenCount;
		}
	}
}
