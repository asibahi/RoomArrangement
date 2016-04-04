namespace RoomArrangement
{
	class Bedroom : Room
	{
		readonly int bedroomID;
		override public string Name => $"Bedroom {bedroomID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int BedroomCount { get; set; }

		public Bedroom(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			bedroomID = ++BedroomCount;
		}
	}
}
