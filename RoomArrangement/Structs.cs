using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override string ToString()
		{
			string s = "(" + X.ToString() + ", " + Y.ToString() + ")";
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

		public override string ToString()
		{
			string s = "(" + XDimension.ToString() + ", " + YDimension.ToString() + ")";
			return s;
		}

	}


}
