using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomArrangement
{
	class House : IList<Room> // On a scale from one to ten, how much do I really need to implement IList?
	{
		// Fields
		readonly List<Room> mainList;
		public List<Room> MainList => mainList;

		public IEnumerable<T> GetRooms<T>() where T : Room => MainList.OfType<T>();

		readonly List<Tuple<Room, Room>> adjacencies;
		public List<Tuple<Room, Room>> Adjacencies => adjacencies;

		public CardinalDirections Streetside { get; private set; }
		public Rectangle Boundary { get; set; }

		public bool CorridorExists { get; internal set; }

		// Constructor.
		public House(CardinalDirections ss)
		{
			mainList = new List<Room>();
			adjacencies = new List<Tuple<Room, Room>>();
			CorridorExists = false;
			Streetside = ss;
		}

		#region IList Implementation
		public Room this[int index]
		{
			get { return MainList[index]; }
			set { MainList[index] = value; }
		}

		public bool Contains(Room item)
		{
			foreach(Room r in MainList)
				if(r.UniqueID == item.UniqueID)
					return true;

			return false;
		}

		public int IndexOf(Room item)
		{
			for(int i = 0; i < Count; i++)
			{
				var r = MainList[i];
				if(r.UniqueID == item.UniqueID)
					return i;
			}

			return -1;
		}

		public int Count => MainList.Count;
		public bool IsReadOnly => true;
		public void Add(Room item) => MainList.Add(item);
		public void Clear() => MainList.Clear();
		public void CopyTo(Room[] array, int arrayIndex) => MainList.CopyTo(array, arrayIndex);
		public void Insert(int index, Room item) => MainList.Insert(index, item);
		public bool Remove(Room item) => MainList.Remove(item);
		public void RemoveAt(int index) => MainList.RemoveAt(index);
		public IEnumerator<Room> GetEnumerator() => MainList.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => MainList.GetEnumerator();
		#endregion

		public void PairRooms(int i, int j) => PairRooms(MainList[i], MainList[j]);
		public void PairRooms(Room r1, Room r2)
		{
			if(r1.UniqueID == r2.UniqueID)
				throw new Exception("Cannot pair a room with itself.");

			foreach(var pair in Adjacencies)
			{
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					throw new Exception("Pair already paired.");
			}

			var adjacentRooms = r1.NumericID < r2.NumericID ? Tuple.Create(r1, r2) : Tuple.Create(r2, r1);
			adjacencies.Add(adjacentRooms);
			Console.WriteLine($"{r1.Name}, {r2.Name} are paired");
		}

		public IEnumerable<Room> GetAdjacentRooms(Room room)
		{
			var rooms = new List<Room>();
			foreach(var pair in adjacencies)
			{
				if(pair.Item1.UniqueID == room.UniqueID)
					rooms.Add(pair.Item2);
				else if(pair.Item2.UniqueID == room.UniqueID)
					rooms.Add(pair.Item1);
			}
			return rooms;
		}

		public bool AreAdjacent(Room r1, Room r2)
		{
			foreach(var pair in adjacencies)
			{
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					return true;
			}

			return false;
		}

		public void AddRoom(RoomType type, string name, Point pt, Rectangle rec)
		{
			Room r = null;
			switch(type)
			{
				case RoomType.Bedroom:
					r = new Bedroom(name, pt, rec);
					break;
				case RoomType.Corridor:
					r = new Corridor(name, pt, rec);
					break;
				case RoomType.Kitchen:
					r = new Kitchen(name, pt, rec);
					break;
				case RoomType.LivingRoom:
					r = new LivingRoom(name, pt, rec);
					break;
				default:
					throw new Exception("This should never happen!! "
						+ "Did you create a new Room type without adding it to the Enum?");
			}

			Add(r);
		}
	}
}