namespace RoomArrangement
{
	public class Bathroom : Room, IWet
	{
		int BathroomID { get; }
		override public string Name => $"Bathroom{BathroomID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int BathroomCount { get; set; }

		protected override bool Flexible => false;

		public Bathroom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			BathroomID = ++BathroomCount;
		}
	}
}
