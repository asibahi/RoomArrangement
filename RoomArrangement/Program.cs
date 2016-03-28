using System;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create the Rooms
			var NumOfRooms = 3;

			for (int i = 0; i < NumOfRooms; i++)
			{
				var r = new Room();
			}

			// This is just to test the code with three rooms
			Database.PairRooms(0, 1);
			Database.PairRooms(0, 2);
			Database.PairRooms(1, 2);

			GACompanions.RunGA(NumOfRooms);

			Console.ReadKey();
		}


	}
}
