namespace RoomArrangement
{
	public class Library : LivingRoom
	{
		int LibraryID { get; }
		override public string Name => $"Library{LibraryID}" + (string.IsNullOrEmpty(name) ? "" : $":{name}");
		static int LibraryCount { get; set; }

		protected override bool Flexible => false;

		public Library(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			LibraryID = ++LibraryCount;
		}
	}
}
