using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	class Input
	{
		public int Sons { get; set; }
		public int Daughters { get; set; }

		private int parents;
		public int Parents
		{
			get { return parents; }
			set { parents = value >= 2 ? 2 : value; }
		}

		private int gparents;
		public int GParents
		{
			get { return gparents; }
			set { gparents = value >= 4 ? 4 : value; }
		}

		private int kpbr = 1;
		public int KidsPerBedroom
		{
			get { return kpbr; }
			set { kpbr = value > 0 ? value : 1; }
		}

		// All dimensions should be dividied by 4 to conform to the 4ft grid.

		public int PlotWidth { get; set; }
		public int PlotDepth { get; set; }

		public int Total => Sons + Daughters + parents + gparents;
	}
}
