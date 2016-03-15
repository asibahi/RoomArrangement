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
		public Rectangle Space { get; set; }
		public Point Anchor { get; set; }
		public Point Corner
		{
			get { return Anchor; }
			set { Anchor = value; }
		}
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
		public int Floor { get; set; }

		// Relational properties
		public List<Room> AdjacentRooms { get; private set; }

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
		public static void PairRooms(Room r1, Room r2)
		{
			r1.AdjacentRooms.Add(r2);
			r2.AdjacentRooms.Add(r1);
		}

		public void Rotate()
		{
			var tempRec = new Rectangle(Space.YDimension, Space.XDimension);
			Space = tempRec;
		}
	}
}
