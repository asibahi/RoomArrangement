using System;
using System.Collections.Generic;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = CardinalDirections.North | CardinalDirections.South;
			var house = new House(input);

			// TODO LiSt:
			//
			// * IMPLEMENT Input AND BldgProgram Functions, and connect them to House
			//
			// * Implement a better way to Pass Streetside to the House, and pass a main street.

			ThrowAndStick.Run(house);
			PushPull.Run(house);

			foreach(Room r in house)
				Console.WriteLine($"{r.Name}'s coordinates are {r.Anchor}. Its dimensions are {r.Space}");

			DrawSolution(house);
			Console.ReadKey();
		}

		// Needs rework
		static void DrawSolution(House house)
		{
			var rooms = new Dictionary<Point, Rectangle>();

			foreach(Room r in house)
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