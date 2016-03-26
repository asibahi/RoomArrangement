namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point(Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public override string ToString()
		{
			string s = "(" + X.ToString() + ", " + Y.ToString() + ")";
			return s;
		}
	}
}
