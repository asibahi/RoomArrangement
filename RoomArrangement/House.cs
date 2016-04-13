using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoomArrangement
{
	class House : IList<Room>
	{
		// Fields and Properties
		readonly List<Room> mainList;
		public List<Room> MainList => mainList;

		public IEnumerable<T> GetRooms<T>() where T : Room => MainList.OfType<T>();

		readonly List<Tuple<Room, Room>> adjacencies;
		public List<Tuple<Room, Room>> Adjacencies => adjacencies;

		public CardinalDirections Streetside { get; private set; }
		public CardinalDirections MainStreet { get; private set; }
		public Rectangle Boundary { get; set; }

		public bool CorridorExists { get; internal set; }

		// Constructor
		public House(Input input, BldgProgram bldgProgram)
		{
			mainList = new List<Room>();
			adjacencies = new List<Tuple<Room, Room>>();
			CorridorExists = false;
			Streetside = input.StreetSides;
			MainStreet = input.MainStreet;

			if(input.MainStreet == CardinalDirections.East || input.MainStreet == CardinalDirections.West)
				Boundary = new Rectangle(input.PlotDepth / 4, input.PlotWidth / 4);
			else
				Boundary = new Rectangle(input.PlotWidth / 4, input.PlotDepth / 4);

			ReadProgram(bldgProgram);
		}



		#region IList Implementation
		// On a scale from one to ten, how much do I really need to implement IList?
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
				if(this[i].UniqueID == item.UniqueID)
					return i;
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

		public void PairRooms(int i, int j) => PairRooms(this[i], this[j]);
		public void PairRooms(Room r1, Room r2)
		{
			if(r1.UniqueID == r2.UniqueID)
				throw new InvalidOperationException("Cannot pair a room with itself.");

			foreach(var pair in Adjacencies)
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					throw new Exception("Pair already paired.");

			var adjacentRooms = r1.NumericID < r2.NumericID ? Tuple.Create(r1, r2) : Tuple.Create(r2, r1);
			adjacencies.Add(adjacentRooms);
			Console.WriteLine($"{r1.Name}, {r2.Name} are paired");
		}

		public IEnumerable<Room> GetAdjacentRooms(Room room)
		{
			var rooms = new List<Room>();
			foreach(var pair in adjacencies)
				if(pair.Item1.UniqueID == room.UniqueID)
					rooms.Add(pair.Item2);
				else if(pair.Item2.UniqueID == room.UniqueID)
					rooms.Add(pair.Item1);
			return rooms;
		}

		public bool AreAdjacent(Room r1, Room r2)
		{
			foreach(var pair in adjacencies)
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					return true;
			return false;
		}

		public void AddRoom<T>(int x, int y) where T : Room => AddRoom<T>(new Rectangle(x, y));
		public void AddRoom<T>(Rectangle rec) where T : Room => AddRoom<T>(null, rec);
		public void AddRoom<T>(string name, int x, int y) where T : Room => AddRoom<T>(name, new Rectangle(x, y));
		public void AddRoom<T>(string name, Rectangle rec) where T : Room => AddRoom<T>(name, Point.Origin, rec);
		public void AddRoom<T>(string name, Point pt, Rectangle rec) where T : Room
		{
			// If documentation of Activator class it to be believed,
			// Should create an instance of T. As if it was // new T(name, pt, rec);
			var room = (T)Activator.CreateInstance(typeof(T), name, pt, rec);
			Add(room);
		}

		void ReadProgram(BldgProgram bldgProgram)
		{
			// Kitchens
			if(bldgProgram.KitchensCount == 1)
				AddRoom<Kitchen>(bldgProgram.KitchenSpace);
			else if(bldgProgram.KitchensCount == 2)
			{
				AddRoom<Kitchen>("Clean", bldgProgram.KitchenSpace);
				AddRoom<Kitchen>("Dirty", bldgProgram.KitchenSpace);
			}
			else
				throw new InvalidOperationException("This should never happen");

			// LivingRooms
			string[] LvTypes = { "Main", "Dining", "Reception", "Library", "Other" };

			for(int i = 0; i < bldgProgram.LivingRoomsCount; i++)
			{
				var name = i < LvTypes.Length ? LvTypes[i] : LvTypes.Last();
				AddRoom<LivingRoom>(name, bldgProgram.LivingRoomSpace);
			}

			// Bedrooms
			for(int i = 0; i < bldgProgram.BedroomOddCount; i++)
				AddRoom<Bedroom>("Older Kids", bldgProgram.BedroomOddSpaces[i]);

			for(int i = 0; i < bldgProgram.BedroomTypCount; i++)
				AddRoom<Bedroom>(bldgProgram.BedroomTypSpaces[i]);

			string[] BrTypes = { "Parents", "GParents", "Second GParents" };
			for(int i = 0; i < bldgProgram.BedroomCouplesCount; i++)
				AddRoom<Bedroom>(BrTypes[i], bldgProgram.BedroomCouplesSpaces[i]);

			if(bldgProgram.TotalNumberOfBedrooms >= 5)
			{
				AddRoom<Corridor>("Bedrooms", 3 * bldgProgram.TotalNumberOfBedrooms, 1);
				CorridorExists = true;
			}
			// This is so bad
		}

		// TODO Needs rework
		public void Draw()
		{
			var rooms = new Dictionary<Point, Rectangle>();

			foreach(Room r in this)
				rooms.Add(r.Anchor, r.Space);

			var roomCounter = 0;
			var recXStart = 21;
			var inRectangle = false;
			var recXCount = 0;
			var currentRec = new Rectangle();
			var currentPnt = new Point();

			// Y loop
			for(int y = 0; y < 20; y++)
			{
				// X loop
				for(int x = 0; x < 20; x++)
				{
					var testPt = new Point(x, y);
					if(rooms.ContainsKey(testPt))
					{
						inRectangle = true;
						roomCounter++;
						recXStart = x;
						currentRec = rooms[testPt];
						currentPnt = testPt;
					}

					if(recXStart == x)
						inRectangle = true;

					if(inRectangle)
					{
						Console.Write("|_");
						recXCount++;
						if(recXCount >= currentRec.XDim)
							inRectangle = false;

					}
					else
					{
						Console.Write(". ");
						recXCount = 0;
					}
				}
				Console.Write("\n");
			}
		}
	}
}