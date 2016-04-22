namespace RoomArrangement
{
	public class Office : Room
	{
		readonly int officeID;
		override public string Name => $"Office{officeID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int OfficeCount { get; set; }

		protected override bool Flexible => false;

		public Office(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			officeID = ++OfficeCount;
		}
	}
}
