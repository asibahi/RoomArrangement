using System;
using System.Collections.Generic;

namespace RoomArrangement
{
	static class Database
	{
		// Fields
		static readonly List<Room> list;
		public static List<Room> List => list;

		static readonly List<Tuple<Room, Room>> adjacencies;
		public static List<Tuple<Room, Room>> Adjacencies => adjacencies;
		// Each Tuple is a pair of rooms. Still unsure 
		// about this and how it would handle duplicates.
		// Would it be better to have the Tuples store IDs instead?

		public static Rectangle Boundary { get; set; }

		// Constructor
		static Database()
		{
			list = new List<Room>();
			adjacencies = new List<Tuple<Room, Room>>();
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


		public static int Count => List.Count;

		public static void Add(Room item) => list.Add(item);
		// Should it check for duplicates?

		public static void Clear() => list.Clear();

		public static bool Contains(Room item) => list.Contains(item);
		// This needs to be rewritten to check for Room ID's instead of the Room ref

		public static void CopyTo(Room[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

		public static IEnumerator<Room> GetEnumerator() => list.GetEnumerator();

		public static int IndexOf(Room item) => list.IndexOf(item); // See comment on Contains()

		public static void Insert(int index, Room item) => list.Insert(index, item);

		public static bool Remove(Room item) => list.Remove(item);

		public static void RemoveAt(int index) => list.RemoveAt(index);
		#endregion

		// Methods for the Database.

		// sy's idea. makes the code "neater".
		public static void ReadRoom(int i,
					     out double recX,
					     out double recY,
					     out double cntX,
					     out double cntY)
		{
			var r = Database.List[i];
			recX = r.Space.XDimension;
			recY = r.Space.YDimension;
			cntX = r.Center.X;
			cntY = r.Center.Y;
		}

		public static void PairRooms(Room r1, Room r2)
		{
			if(r1.ID == r2.ID)
				throw new Exception("Cannot pair a room with itself.");

			foreach(var pair in Adjacencies)
			{
				if((r1.ID == pair.Item1.ID && r2.ID == pair.Item2.ID)
				    || (r1.ID == pair.Item2.ID && r2.ID == pair.Item1.ID))
					throw new Exception("Pair already paired.");
			}

			var adjacentRooms = r1.ID < r2.ID ? Tuple.Create(r1, r2) : Tuple.Create(r2, r1);
			adjacencies.Add(adjacentRooms);
			Console.WriteLine("Rooms {0} and {1} are paired", r1.Name, r2.Name);
		}

		public static void PairRooms(int i, int j) => PairRooms(Database.List[i], Database.List[j]);

		public static List<Room> GetAdjacentRooms(Room room)
		{
			var rooms = new List<Room>();
			foreach(var pair in adjacencies)
			{
				if(pair.Item1.ID == room.ID)
					rooms.Add(pair.Item2);
				else if(pair.Item2.ID == room.ID)
					rooms.Add(pair.Item1);
			}
			return rooms;
		}

		public static bool AreAdjacent(Room r1, Room r2)
		{
			foreach(var pair in adjacencies)
			{
				if((r1.ID == pair.Item1.ID && r2.ID == pair.Item2.ID)
				    || (r1.ID == pair.Item2.ID && r2.ID == pair.Item1.ID))
					return true;
			}

			return false;
		}
	}
}