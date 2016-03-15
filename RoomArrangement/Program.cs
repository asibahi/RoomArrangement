using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
		}
	}


	// Code begin here

	// Replicating shit from the Rhino SDK for my purposes
	struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override string ToString()
		{
			string s = "(" + X.ToString() + ", " + Y.ToString() + " )";
			return s;
		}

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
	}

	struct Rectangle
	{
		public int XDimension { get; set; }
		public int YDimension { get; set; }

		public Rectangle(int x, int y)
		{
			XDimension = x;
			YDimension = y;
		}

		public Rectangle(Rectangle r)
		{
			XDimension = r.XDimension;
			YDimension = r.YDimension;
		}
	}

	class Room
	{

		// Meta properties
		public int ID { get; private set; }
		public static int Population { get; private set; }
		public string Name { get; private set; }

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

		// Relational properties
		public List<Room> AdjacentRooms { get; private set; }

		// Constuctor
		public Room(string s, Point pt, Rectangle rec)
		{
			ID = ++Population;
			Name = s;

			Space = rec;
			Anchor = pt;
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
