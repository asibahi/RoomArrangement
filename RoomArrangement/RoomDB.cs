using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	static class RoomDB
	{
		// List and related methods
		static public List<Room> List { get; private set; }

		static public void Add(Room r)
		{
			List.Add(r);
		}

		static public int Count
		{
			get { return List.Count; }
		}

		// Constructor
		static RoomDB()
		{
			List = new List<Room>();
		}

		// Room edits
		public static void PairRooms(Room r1, Room r2)
		{
			r1.AdjacentRooms.Add(r2);
			r2.AdjacentRooms.Add(r1);
		}
	}
}
}