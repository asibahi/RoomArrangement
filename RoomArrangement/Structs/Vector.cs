using System;

namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Vector
	{
		public int X { get; set; }
		public int Y { get; set; }

		// Might want to use doubles instead of integers so for more precision and stuff
		// We'll see how that plays out when implementing a Move method.
		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector(Vector v)
		{
			X = v.X;
			Y = v.Y;
		}

		public Vector(Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public Vector(Point p1,Point p2)
			: this(p2 - p1)
		{
		}

		public double Length => Math.Sqrt((X * X) + (Y * Y));
		public double Angle => Math.Atan(Y / X);

		public override string ToString() => "V(" + X.ToString() + ", " + Y.ToString() + ")";

		#region Fun Stuff
		// Addition and subtraction.
		public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y);
		public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y);

		// Equality
		public static bool operator ==(Vector v1, Vector v2) => v1.X == v2.X && v1.Y == v2.Y;
		public static bool operator !=(Vector v1, Vector v2) => !(v1.X == v2.X && v1.Y == v2.Y);

		public override bool Equals(object obj) => obj is Vector && this == (Vector)obj;
		public bool Equals(Vector v) => ((X == v.X) && (Y == v.Y));
		public override int GetHashCode() => X ^ (13 * Y);

		// Negation
		public static Vector operator !(Vector v) => new Vector(0 - v.X, 0 - v.Y); 
		#endregion

	}
}
