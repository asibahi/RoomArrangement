namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Rectangle
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

		public override string ToString() => $"[{XDim}, {YDim}]";
	}
}
