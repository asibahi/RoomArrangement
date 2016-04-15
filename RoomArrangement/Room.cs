using System;
using static System.Math;

namespace RoomArrangement
{
	abstract class Room
	{
		// Meta properties
		readonly int numericID;
		protected readonly string name;
		private readonly Guid uniqueID;

		public int NumericID => numericID; // Used for ordering in Tuples.
		abstract public string Name { get; }
		public Guid UniqueID => uniqueID;
		static int TotalRoomCount { get; set; }

		// Geometric properties
		// Note the Anchor here is supposed to be the SW Corner.
		public Rectangle Space { get; private set; }
		public Point Anchor { get; private set; }
		public char Orientation => Space.XDim == Space.YDim ? 'O' : (Space.XDim > Space.YDim ? 'X' : 'Y');
		public Point Center
		{
			get
			{
				var pt = new Point(Anchor);
				pt.X += Space.XDim / 2;
				pt.Y += Space.YDim / 2;
				return pt;
			}
		}

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
		public void Rotate() => Space = new Rectangle(Space.YDim, Space.XDim);

		public void Adjust(int x, int y, bool YOrientation) => Adjust(new Point(x, y), YOrientation);
		public void Adjust(Point pt, bool YOrientation)
		{
			// Setting the new Anchor
			Anchor = pt;

			// Setting the new Orientation
			// 0 is X, 1 is Y. Feels better this way, but doesn't really matter.
			char tempChar = YOrientation ? 'Y' : 'X';

			if(Orientation != 'O' && tempChar != Orientation)
				Rotate();
		}

		public void Move(Vector v) => Anchor += new Point((int)Ceiling(v.X), (int)Ceiling(v.Y));
		public void Move(int x, int y) => Move(new Vector(x, y));

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
	}
}