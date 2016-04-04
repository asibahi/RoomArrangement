using static System.Math;

namespace RoomArrangement
{
	abstract class Room
	{
		// Meta properties
		readonly int roomUniqueID;
		public int RoomUniqueID => roomUniqueID;
		public int Index => roomUniqueID - 1;

		protected readonly string name;
		abstract public string Name { get; }

		static int TotalRoomCount { get; set; }

		// Geometric properties
		// Note the Anchor here is supposed to be the SW Corner.
		public Rectangle Space { get; private set; }
		public Point Anchor { get; private set; }
		public char Orientation => Space.XDimension == Space.YDimension ? 'O' : (Space.XDimension > Space.YDimension ? 'X' : 'Y');
		public Point Center
		{
			get
			{
				var pt = new Point(Anchor);
				pt.X += Space.XDimension / 2;
				pt.Y += Space.YDimension / 2;
				return pt;
			}
		}

		// Empty Constructor
		public Room() 
			: this(null, new Point(), new Rectangle(3, 4))
		{
		}

		// Constuctor
		public Room(string n, Point pt, Rectangle rec)
		{
			roomUniqueID = ++TotalRoomCount;
			name = n;

			Space = rec;
			Anchor = pt;

			Database.Add(this);
		}

		// Methods and stuff
		public void Rotate() => Space = new Rectangle(Space.YDimension, Space.XDimension);

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
	}
}
