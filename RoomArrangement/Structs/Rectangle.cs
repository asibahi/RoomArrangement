namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Rectangle
	{
		public int XDim { get; set; }
		public int YDim { get; set; } 

		public Rectangle(int x, int y)
		{
			XDim = x;
			YDim = y;
		}

		public Rectangle(Rectangle r)
		{
			XDim = r.XDim;
			YDim = r.YDim;
		}

		public int Area => XDim * YDim;
		public int Circumfrance => 2 * XDim + 2 * YDim;

		public override string ToString() => $"[{XDim}, {YDim}]";
	}
}
