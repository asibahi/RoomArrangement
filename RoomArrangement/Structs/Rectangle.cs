namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Rectangle
	{
		public int XDimension { get; set; }
		public int YDimension { get; set; }

		public Rectangle(int x, int y)
		{
			XDimension = x;
			YDimension = y;
		}

		public Rectangle(Rectangle r)
		{
			XDimension = r.XDimension;
			YDimension = r.YDimension;
		}

		public int Area => XDimension * YDimension;
		public int Circumfrance => 2 * XDimension + 2 * YDimension;

		public override string ToString() => "(" + XDimension.ToString() + ", " + YDimension.ToString() + ")";
	}
}
