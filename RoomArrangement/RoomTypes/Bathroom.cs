namespace RoomArrangement
{
	public class Bathroom : Room, IWet
	{
		readonly int bathroomID;
		override public string Name => $"Bathroom {bathroomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int BathroomCount { get; set; }

		protected override bool Flexible => false;

		public Bathroom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			bathroomID = ++BathroomCount;
		}
	}
}
