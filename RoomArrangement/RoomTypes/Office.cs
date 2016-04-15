namespace RoomArrangement
{
	class Office : Room
	{
		readonly int officeID;
		override public string Name => $"Office {officeID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int OfficeCount { get; set; }

		public Office(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			officeID = ++OfficeCount;
		}
	}
}
