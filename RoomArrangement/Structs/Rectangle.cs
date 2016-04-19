namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	public struct Rectangle
	{
		public double XDim { get; set; }
		public double YDim { get; set; }

		public Rectangle(double x, double y)
		{
			XDim = x;
			YDim = y;
		}

		public Rectangle(Rectangle r)
		{
			XDim = r.XDim;
			YDim = r.YDim;
		}

		public double Area => XDim * YDim;
		public double Circumfrance => 2 * XDim + 2 * YDim;

		public char SmallerSide => XDim == YDim ? 'O' : (XDim < YDim ? 'X' : 'Y');
		public char LargerSide => XDim == YDim ? 'O' : (XDim > YDim ? 'X' : 'Y');

		public Rectangle ExtendLargerBy(double length) =>
			LargerSide == 'X' ? new Rectangle(XDim + length, YDim)
					  : new Rectangle(XDim, YDim + length);

		public Rectangle ExtendSmallerBy(double length) =>
			SmallerSide == 'X' ? new Rectangle(XDim + length, YDim)
					   : new Rectangle(XDim, YDim + length);

		public static Rectangle Rotate(Rectangle rec) => new Rectangle(rec.YDim, rec.XDim);

		// Equality
		public static bool operator ==(Rectangle r1, Rectangle r2) => r1.Equals(r2);
		public static bool operator !=(Rectangle r1, Rectangle r2) => !r1.Equals(r2);

		public bool Equals(Rectangle r) => (XDim == r.XDim) && (YDim == r.YDim);
		public override bool Equals(object obj) => obj is Rectangle && this == (Rectangle)obj;
		public override int GetHashCode() => XDim.GetHashCode() ^ (23 * YDim.GetHashCode());

		public override string ToString() => $"[{XDim}, {YDim}]";
	}
}