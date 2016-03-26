using System;
using System.Collections.Generic;

namespace RoomArrangement
{
	static class Database
	{
		// Fields
		public static List<Room> List { get; private set; }
		public static List<Tuple<Room, Room>> Adjacacencies
		{
			// Each Tuple is a pair of rooms. Still unsure 
			// about this and how it would handle duplicates.
			// Would it be better to have the Tuples store IDs instead?
			//
			get;
			private set;
		}

		// Constructor
		static Database()
		{
			List = new List<Room>();
			Adjacacencies = new List<Tuple<Room, Room>>();
		}

		#region Following Default IList implementation. See comments and Todo inside.
		// Copying the IList implementation w/o Indxer
		// as I cannot have Indexers in static classes.
		//
		// Alternative would be to have an Indexer as Follows: 
		//
		// public Room this[int index]
		// {
		//	get { return list[index]; }
		//	set { list[index] = value; }
		// }
		// 
		// and create an instance of the class in Program while remembering
		// to add every Room as it is created to the list.
		// Current code implementation have Rooms add themselves to the list in
		// the Room() constructor. So I am less likely to forget a Room. 
		//
		// However, if I were to get the program to design multiple houses
		// the IList interface implementation might prove useful if I were
		// to create a separate roomDatabase for every house.
		//
		// Todo : Revise Contains() and IndexOf() to check for ID instead.


		public static int Count
		{
			get { return List.Count; }
		}

		public static void Add(Room item)
		{
			List.Add(item);
			// Should it check for duplicates?
		}

		public static void Clear()
		{
			List.Clear();
		}

		public static bool Contains(Room item)
		{
			// This needs to be rewritten to check for Room ID's instead of the Room ref
			return List.Contains(item);
		}

		public static void CopyTo(Room[] array, int arrayIndex)
		{
			List.CopyTo(array, arrayIndex);
		}

		public static IEnumerator<Room> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		public static int IndexOf(Room item)
		{
			// See comment on Contains()
			return List.IndexOf(item);
		}

		public static void Insert(int index, Room item)
		{
			List.Insert(index, item);
		}

		public static bool Remove(Room item)
		{
			return List.Remove(item);
		}

		public static void RemoveAt(int index)
		{
			List.RemoveAt(index);
		}
		#endregion

		// Methods for the Database.

		public static void PairRooms(Room r1, Room r2)
		{
			if (r1.ID == r2.ID)
				throw new Exception("Cannot pair a room with itself.");

			foreach (var pair in Adjacacencies)
			{
				if ((r1.ID == pair.Item1.ID && r2.ID == pair.Item2.ID)
				    || (r1.ID == pair.Item2.ID && r2.ID == pair.Item1.ID))
					throw new Exception("Pair already paired.");
			}

			var adjacentRooms = new Tuple<Room, Room>(r1, r2);
			Adjacacencies.Add(adjacentRooms);
			Console.WriteLine("Rooms {0} and {1} are paired", r1.Name, r2.Name);
		}
		public static void PairRooms(int i, int j)
		{
			var r1 = Database.List[i];
			var r2 = Database.List[j];
			PairRooms(r1, r2);
		}

		public static List<Room> GetAdjacentRooms(Room room)
		{
			var rooms = new List<Room>();
			foreach (var pair in Adjacacencies)
			{
				if (pair.Item1.ID == room.ID)
					rooms.Add(pair.Item2);
				else if (pair.Item2.ID == room.ID)
					rooms.Add(pair.Item1);
			}
			return rooms;
		}

		public static bool AreAdjacent(Room r1, Room r2)
		{
			foreach (var pair in Adjacacencies)
			{
				if ((r1.ID == pair.Item1.ID && r2.ID == pair.Item2.ID)
				    || (r1.ID == pair.Item2.ID && r2.ID == pair.Item1.ID))
					return true;
			}

			return false;
		}
	}
}