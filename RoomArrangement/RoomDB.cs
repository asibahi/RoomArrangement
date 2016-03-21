using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	static class Database
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
		static Database()
		{
			List = new List<Room>();
		}

		// Room edits
		public static void PairRooms(Room r1, Room r2)
		{
			if (r1.ID != r2.ID)
			{
				r1.AdjacentRooms.Add(r2);
				r2.AdjacentRooms.Add(r1);
				Console.WriteLine(string.Format("{0} and {1} are paired.", r1.Name, r2.Name));
			}
		}

		// Room edits
		public static void PairRooms(int i1, int i2)
		{
			var r1 = List[i1];
			var r2 = List[i2];
			PairRooms(r1, r2);
		}
	}
}
