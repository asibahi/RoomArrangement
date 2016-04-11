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

			house.RunThrowAndStick();
			house.RunPushPull();

			foreach(Room r in house)
				Console.WriteLine($"{r.Name}'s coordinates are {r.Anchor}. Its dimensions are {r.Space}");

			house.Draw();
			Console.ReadKey();
		}
	}
}