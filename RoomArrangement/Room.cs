using System;
using static System.Math;

namespace RoomArrangement
{
	public abstract class Room : IEquatable<Room>
	{
		// Meta properties
		readonly int numericID;
		protected readonly string name;
		private readonly Guid uniqueID;

		public int NumericID => numericID; // Used for ordering in Tuples.
		abstract public string Name { get; }
		public Guid UniqueID => uniqueID;
		static int TotalRoomCount { get; set; }

		protected abstract bool Flexible { get; }

		// Geometric properties
		// Note the Anchor here is supposed to be the SW Corner.
		public Rectangle Space { get; private set; }
		public Point Anchor { get; private set; }
		public char Orientation => Space.LargerSide;
		public Point Center => Anchor + new Point(Space.XDim / 2, Space.YDim / 2);
		public double Area => Space.Area;

		double SmallerSideSize => Min(Space.XDim, Space.YDim);
		double LargerSideSize => Max(Space.XDim, Space.YDim);

		// Constructor
		protected Room(string n, Point pt, Rectangle rec)
		{
			numericID = ++TotalRoomCount;
			uniqueID = Guid.NewGuid();
			name = n;

			Space = rec;
			Anchor = pt;
		}

		// Methods and stuff
		public void Rotate() => Space = Rectangle.Rotate(Space);

		public void Move(Vector v) => Anchor += new Point((int)Ceiling(v.X), (int)Ceiling(v.Y));
		public void Move(int x, int y) => Move(new Vector(x, y));

		public void MoveTo(Point pt) => Anchor = pt;

		public void Adjust(int x, int y, bool YOrientation) => Adjust(new Point(x, y), YOrientation);
		public void Adjust(Point pt, bool YOrientation)
		{
			Anchor = pt;
			char tempChar = YOrientation ? 'Y' : 'X'; // doesn't really matter which is which

			if(Orientation != 'O' && tempChar != Orientation)
				Rotate();
		}

		public void Read(out double recX,
				out double recY,
				out double cntX,
				out double cntY)
		{
			recX = Space.XDim;
			recY = Space.YDim;
			cntX = Center.X;
			cntY = Center.Y;
		}

		public void ExtendLength(double area)
		{
			if(!Flexible)
				throw new InvalidOperationException("This room cannot be expanded.");

			var length = Round(area / SmallerSideSize);
			Space = Space.ExtendLargerBy(length);
		}

		// Equality
		public static bool operator ==(Room r1, Room r2) => r1.Equals(r2);
		public static bool operator !=(Room r1, Room r2) => !r1.Equals(r2);

		public bool Equals(Room other) => UniqueID == other.UniqueID;
		public override bool Equals(object obj) => obj is Room && this == (Room)obj;
		public override int GetHashCode() => UniqueID.GetHashCode();

		public override string ToString() => $"{GetType().Name}: {Name} at {Anchor} with dimensions {Space}";
	}
}