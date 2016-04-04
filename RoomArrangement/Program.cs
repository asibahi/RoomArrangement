using System;
using System.Collections.Generic;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create the Rooms
			var NumOfRooms = 3;

			for(int i = 0; i < NumOfRooms; i++)
				new Room();


			// This is just to test the code with three rooms
			Database.PairRooms(0, 1);
			Database.PairRooms(0, 2);
			Database.PairRooms(1, 2);

			// this is 16 * 4 ft and 24 * 4 ft. Roughly equal to 20 by 30 sqm.
			// Because my glorious app works on a grid of 4ft.
			Database.Boundary = new Rectangle(16, 25);

			ThrowAndStick.Run(NumOfRooms);

			PushPull.Run();

			foreach(Room r in Database.List)
				Console.WriteLine($"{r.Name}'s coordinates are {r.Anchor}. Its dimensions are {r.Space}");

			DrawSolution();
			Console.ReadKey();
		}

		// Needs rework
		static void DrawSolution()
		{
			var rooms = new Dictionary<Point, Rectangle>();

			foreach(Room r in Database.List)
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
						if(recXCount >= currentRec.XDimension)
						{
							inRectangle = false;
						}
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
