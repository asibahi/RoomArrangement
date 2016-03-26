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
		private readonly int id;
		public int ID => id;

		private readonly string name;
		public string Name => string.Format(
					string.IsNullOrEmpty(name)
					? "Room {1}"
					: "Room {1} : {0}", name, ID);

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

		// Empty Constructor
		public Room()
			: this(null, new Point(), new Rectangle(3, 4))
		{
		}

		// Constuctor
		public Room(string n, Point pt, Rectangle rec)
		{
			id = ++Population;
			name = n;

			Space = rec;
			Anchor = pt;

			Database.Add(this);
		}


		// Methods and stuff

		public void Rotate()
		{
			Space = new Rectangle(Space.YDimension, Space.XDimension);
		}

		public void Adjust(int x, int y, bool YOrientation)
		{
			// Setting the new Anchor
			Anchor = new Point(x, y);

			// Setting the new Orientation
			// 0 is X, 1 is Y. Feels better this way, but doesn't really matter.

			char tempChar = YOrientation ? 'Y' : 'X';

			if (Orientation != 'O' && tempChar != Orientation)
				Rotate();
		}
	}
}
