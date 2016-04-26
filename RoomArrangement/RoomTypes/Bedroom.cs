namespace RoomArrangement
{
	public class Bedroom : Room
	{
		int BedroomID { get; }
		override public string Name => $"Bedroom{BedroomID}" + (string.IsNullOrEmpty(name) ? "": $":{name}");
		static int BedroomCount { get; set; }

		protected override bool Flexible => false;

		public Bedroom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			BedroomID = ++BedroomCount;
		}
	}
}
