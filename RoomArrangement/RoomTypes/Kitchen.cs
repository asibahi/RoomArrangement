namespace RoomArrangement
{
	public class Kitchen : Room, IWet
	{
		int KitchenID { get; }
		override public string Name => $"Kitchen{KitchenID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int KitchenCount { get; set; }

		protected override bool Flexible => false;

		public Kitchen(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			KitchenID = ++KitchenCount;
		}
	}
}
