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

		public static Point Origin = new Point();

		public override string ToString() => $"({X}, {Y})";

		#region Fun Stuff
		// Addition and subtraction.
		public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
		public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

		// Equality
		public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;
		public static bool operator !=(Point p1, Point p2) => !(p1.X == p2.X && p1.Y == p2.Y);

		public override bool Equals(object obj) => obj is Point && this == (Point)obj;
		public bool Equals(Point p) => ((X == p.X) && (Y == p.Y));
		public override int GetHashCode() => X ^ (7 * Y);

		// Negation
		public static Point operator !(Point p) => new Point(0 - p.X, 0 - p.Y);
		#endregion
	}
}