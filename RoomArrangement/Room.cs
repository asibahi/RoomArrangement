using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	class Room
	{

		// Meta properties
		public int ID { get; private set; }
		public string Name { get; private set; }
		private static int Population { get; set; }

		// Geometric properties
		// Note the Anchor here is supposed to be the SW Corner.
		public Rectangle Space { get; set; }
		public Point Anchor { get; set; }
		public char Orientation
		{
			get
			{
				if (Space.XDimension > Space.YDimension)
					return 'X';
				else if (Space.XDimension < Space.YDimension)
					return 'Y';
				else
					return 'O';
			}
		}
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

		// Relational properties
		public List<Room> AdjacentRooms { get; private set; }

		// Empty Constructor
		public Room()
		{
			ID = ++Population;
			Name = string.Format("Room Number {0}", ID);

			Space = new Rectangle();
			Anchor = new Point();

			RoomDB.Add(this);
		}

		// Constuctor
		public Room(string s, Point pt, Rectangle rec)
		{
			ID = ++Population;
			Name = s;

			Space = rec;
			Anchor = pt;

			RoomDB.Add(this);
		}


		// Methods and stuff

		public void Rotate()
		{
			var tempRec = new Rectangle(Space.YDimension, Space.XDimension);
			Space = tempRec;
		}

		public void Adjust(int x, int y, bool YOrientation)
		{
			// Setting the new Anchor
			var tempPt = new Point(x, y);
			Anchor = tempPt;

			// Setting the new Orientation
			// 0 is X, 1 is Y. Feels better this way, but doesn't really matter.

			char tempChar = YOrientation ? 'Y' : 'X';

			if (Orientation != 'O' && tempChar != Orientation)
				Rotate();
		}
	}
}
