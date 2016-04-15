namespace RoomArrangement
{
	class Library : LivingRoom
	{
		readonly int libraryID;
		override public string Name => $"Library {libraryID}" + (string.IsNullOrEmpty(name) ? "" : $" : {name}");
		static int LibraryCount { get; set; }

		public Library(string n, Point pt, Rectangle rec) : base(n, pt, rec)
		{
			libraryID = ++LibraryCount;
		}
	}
}
