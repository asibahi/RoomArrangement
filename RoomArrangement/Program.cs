using System;
using System.Collections.Generic;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			var streetSide = CardinalDirections.North | CardinalDirections.South;
			var house = new House(streetSide);

			var input = new Input();

			// Ask for Input here

			var bldgProgram = new BldgProgram(input, house);

			house.RunThrowAndStick();
			house.RunPushPull();

			foreach(Room r in house)
				Console.WriteLine($"{r.Name}'s coordinates are {r.Anchor}. Its dimensions are {r.Space}");

			house.Draw();
			Console.ReadKey();
		}
	}
}