namespace RoomArrangement
{
	public class Office : Room
	{
		int OfficeID { get; }
		override public string Name => $"Office{OfficeID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int OfficeCount { get; set; }

		protected override bool Flexible => false;

		public Office(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			OfficeID = ++OfficeCount;
		}
	}
}
