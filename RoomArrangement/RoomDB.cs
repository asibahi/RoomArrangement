using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	static class RoomDB
	{
		static public List<Room> List { get; private set; }

		static public void Add(Room r)
		{
			List.Add(r);
		}

		static int Count
		{
			get { return List.Count; }
		}
	}
}


