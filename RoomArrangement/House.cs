using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace RoomArrangement
{
	public class House : IList<Room>
	{
		// Fields and Properties
		List<Room> mainList;

		public List<T> GetRooms<T>() where T : Room => mainList.OfType<T>().ToList();

		public List<Tuple<Room, Room>> Adjacencies { get; private set; }

		public CardinalDirections Streetside { get; private set; }
		public CardinalDirections MainStreet { get; private set; }
		public Rectangle Boundary { get; set; }

		public double GridSize { get; private set; } = 4;

		// Constructor, of the House, no irony whatsoever
		public House(Input input, BldgProgram bldgProgram)
		{
			mainList = new List<Room>();
			Adjacencies = new List<Tuple<Room, Room>>();
			Streetside = input.StreetSides;
			MainStreet = input.MainStreet;

			var privatePool = bldgProgram.PrivatePool / (GridSize * GridSize);
			var publicPool = bldgProgram.PublicPool / (GridSize * GridSize);

			var bedroomCount = bldgProgram.BedroomCount;

			var kitchenPool = bldgProgram.KitchenPool / (GridSize * GridSize);

			if(input.MainStreet == CardinalDirections.East || input.MainStreet == CardinalDirections.West)
				Boundary = new Rectangle(input.PlotDepth / GridSize, input.PlotWidth / GridSize);
			else
				Boundary = new Rectangle(input.PlotWidth / GridSize, input.PlotDepth / GridSize);

			var main = AddRoom<LivingRoom>("Main", 12 / GridSize, 16 / GridSize);
			privatePool -= main.Area;

			Room livingRoom;

			if(privatePool > (24 / GridSize) * (12 / GridSize))
			{
				livingRoom = AddRoom<LivingRoom>(null, 24 / GridSize, 12 / GridSize);
				privatePool -= livingRoom.Area;

				PairRooms(main, livingRoom);
			}
			else
			{
				main.ExtendLength(privatePool);
				livingRoom = main;
			}

			#region BedroomCreation
			if(input.Grandparents > 0)
			{
				// Technically this should be a suite
				var gparentsbedroom = AddRoom<Bedroom>("Grandparents", 12 / GridSize, 20 / GridSize);
				bedroomCount--;

				PairRooms(gparentsbedroom, livingRoom);
			}

			Room bedroomHub;

			if(bedroomCount > 1)
			{
				bedroomHub = AddRoom<Corridor>("Bedroom Corridor", 4 / GridSize, ((bedroomCount * 6) + 6) / GridSize);
				PairRooms(bedroomHub, livingRoom);
			}
			else
				bedroomHub = livingRoom;

			if(input.Grandparents > 2)
			{
				// Technically this should be a suite
				var gparentsbedroom = AddRoom<Bedroom>("Grandparents 2", 12 / GridSize, 20 / GridSize);
				bedroomCount--;

				PairRooms(gparentsbedroom, bedroomHub);
			}

			if(input.Parents > 0)
			{
				var parentsBedroom = AddRoom<Bedroom>("Parents", 12 / GridSize, 20 / GridSize);
				bedroomCount--;

				PairRooms(parentsBedroom, bedroomHub);
			}

			foreach(int residentFactor in bldgProgram.NumberOfKidsInTheOlderKidsBedroom)
				if(residentFactor != 0)
					if(residentFactor <= 2) // Odd rooms.
					{
						var bedroom = AddRoom<Bedroom>("Older Kids", 16 / GridSize, 12 / GridSize);
						bedroomCount--;

						PairRooms(bedroom, bedroomHub);
					}
					else
					{
						var bedroom = AddRoom<Bedroom>("Older Kids", ((4 * (residentFactor - 1)) + 12) / GridSize, 12 / GridSize);
						bedroomCount--;

						PairRooms(bedroom, bedroomHub);
					}

			for(int i = 0; i < bedroomCount; i++)
			{
				var bedroom = AddRoom<Bedroom>(((4 * input.KidsPerBedroom) + 8) / GridSize, 12 / GridSize);
				PairRooms(bedroom, bedroomHub);
			}
			#endregion

			var desiredRooms = input.Rooms;
			Room diningRoom;

			if(desiredRooms.HasFlag(InputRooms.DiningRoom)) // Sketch
			{
				var diningRoomSize = input.Total <= 8 ? 16
								      : Floor((input.Total - 8d) / 4) * 4 + 16;
				// 12 * 16 for 8 people !! more for each extra 4 ppl add 4 ft.
				diningRoom = AddRoom<DiningRoom>("Dining", 12 / GridSize, diningRoomSize / (12 * GridSize));
				publicPool -= diningRoom.Area;

				PairRooms(diningRoom, livingRoom);
			}
			else
			{
				livingRoom.ExtendLength(3);
				diningRoom = livingRoom;
			}

			#region Kitchen Creation
			int[] dimsForKitchens = { 8, 12, 16 }; // this part should be part of criteria

			var resFactor = (int)(Ceiling(input.Total / 2d) - 1);
			var dirtyWanted = desiredRooms.HasFlag(InputRooms.DirtyKitchen);
			var cleanWanted = desiredRooms.HasFlag(InputRooms.CleanKitchen);

			if(bldgProgram.Kitchenette)
				livingRoom.ExtendLength(4); // this seems wrong. my livingroom will be infinite

			Room dirtyKitchen;
			Room cleanKitchen;

			if(dirtyWanted && cleanWanted)
				if(resFactor < dimsForKitchens.Count())
				{
					dirtyKitchen = AddRoom<Kitchen>("Dirty", dimsForKitchens[resFactor] / GridSize, 12 / GridSize);
					cleanKitchen = livingRoom;
				}
				else
				{
					dirtyKitchen = AddRoom<Kitchen>("Dirty", dimsForKitchens[resFactor] / GridSize, 12 / GridSize);
					cleanKitchen = AddRoom<Kitchen>("Clean", dimsForKitchens[resFactor] / GridSize, 12 / GridSize);
				}

			else if(dirtyWanted ^ cleanWanted) // XOR operator FTW
				dirtyKitchen = cleanKitchen = AddRoom<Kitchen>(dimsForKitchens[resFactor] / GridSize, 12 / GridSize);
			else
				dirtyKitchen = cleanKitchen = livingRoom; // Kitchenette
			#endregion

			#region Desired Rooms 

			// Ideally the code would cycle through those by the priority made by the client. or CRITERIA
			// Check how many flags are satisfied. If the flags are more than a certain percentage: create a corridor/or private rooms hub, and connect the extra rooms to it.
			// alternatively, if livingRoom has more than a certain number of connection, it is extended per extra connection.
			// TODO TODO TODO
			//
			// if(publicPool > 0 && desiredRooms.HasFlag(InputRooms.Reception))
			// {
			//	int receptionHallArea = Function(input.Total); // What would that be?
			//	var reception = AddRoom<Reception>(12 / GridSize, receptionHallArea / (12 * GridSize));

			//	PairRooms(main, reception);

			//	publicPool -= reception.Area;

			//	if(publicPool > receptionHallArea)
			//	{
			//		// new Reception Room for te other gender .. maybe?
			//	}
			//	else
			//	{
			//		reception.ExtendLength(publicPool);
			//		// There should be an entrypoint variable where I assign to the street in the end.
			//	}
			// }

			// if(privatePool > 0 && desiredRooms.HasFlag(InputRooms.Library))
			// {

			// }

			// Go through rooms in desiredRooms. Check if privatePool has enough space.
			// Library
			// Office
			// GameRoom
			// w/e 
			//
			// Ideally the code would cycle through those by the priority made by the client.

			#endregion
		}

		#region IList Implementation
		// On a scale from one to ten, how much do I really need to implement IList?
		public Room this[int index]
		{
			get { return mainList[index]; }
			set { mainList[index] = value; }
		}

		public bool Contains(Room item)
		{
			foreach(Room r in mainList)
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

		public int Count => mainList.Count;
		public bool IsReadOnly => true;

		public void Add(Room item) => mainList.Add(item);
		public void Clear() => mainList.Clear();
		public void CopyTo(Room[] array, int arrayIndex) => mainList.CopyTo(array, arrayIndex);
		public void Insert(int index, Room item) => mainList.Insert(index, item);
		public bool Remove(Room item) => mainList.Remove(item);
		public void RemoveAt(int index) => mainList.RemoveAt(index);
		public IEnumerator<Room> GetEnumerator() => mainList.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => mainList.GetEnumerator();
		#endregion

		public void PairRooms(Room r1, Room r2)
		{
			if(r1.UniqueID == r2.UniqueID)
				throw new InvalidOperationException("Cannot pair a room with itself.");

			foreach(var pair in Adjacencies)
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					throw new Exception("Pair already paired.");

			var adjacentRooms = r1.NumericID < r2.NumericID ? Tuple.Create(r1, r2) : Tuple.Create(r2, r1);
			Adjacencies.Add(adjacentRooms);
			Console.WriteLine($"{r1.Name}, {r2.Name} are paired");
		}

		public IList<Room> GetAdjacentRooms(Room room)
		{
			var rooms = new List<Room>();
			foreach(var pair in Adjacencies)
				if(pair.Item1.UniqueID == room.UniqueID)
					rooms.Add(pair.Item2);
				else if(pair.Item2.UniqueID == room.UniqueID)
					rooms.Add(pair.Item1);
			return rooms;
		}

		public bool AreAdjacent(Room r1, Room r2)
		{
			foreach(var pair in Adjacencies)
				if((r1.UniqueID == pair.Item1.UniqueID && r2.UniqueID == pair.Item2.UniqueID)
				    || (r1.UniqueID == pair.Item2.UniqueID && r2.UniqueID == pair.Item1.UniqueID))
					return true;
			return false;
		}

		public T AddRoom<T>(double x, double y) where T : Room => AddRoom<T>(new Rectangle(x, y));
		public T AddRoom<T>(Rectangle rec) where T : Room => AddRoom<T>(null, rec);
		public T AddRoom<T>(string name, double x, double y) where T : Room => AddRoom<T>(name, new Rectangle(x, y));
		public T AddRoom<T>(string name, Rectangle rec) where T : Room => AddRoom<T>(name, Point.Origin, rec);
		public T AddRoom<T>(string name, Point pt, Rectangle rec) where T : Room
		{
			// Should create an instance of T. As if it was // new T(name, pt, rec);
			var room = (T)Activator.CreateInstance(typeof(T), name, pt, rec);
			Add(room);
			return room; // so I can assign this room to variables and pair it and stuff.
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