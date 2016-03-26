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

		public override string ToString()
		{
			string s = "(" + XDimension.ToString() + ", " + YDimension.ToString() + ")";
			return s;
		}
	}
}
