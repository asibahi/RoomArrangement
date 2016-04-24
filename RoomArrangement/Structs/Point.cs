using System;

namespace RoomArrangement
{
	public struct Point : IEquatable<Point>
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x, double y)
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
		public static bool operator ==(Point p1, Point p2) => p1.Equals(p2);
		public static bool operator !=(Point p1, Point p2) => !p1.Equals(p2);

		public bool Equals(Point p) => (X == p.X) && (Y == p.Y);
		public override bool Equals(object obj) => obj is Point && this == (Point)obj;
		public override int GetHashCode() => X.GetHashCode() ^ (7 * Y.GetHashCode());

		// Negation
		public static Point operator -(Point p) => new Point(0 - p.X, 0 - p.Y);
		#endregion
	}
}