using System;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new Input();

			// Ask for Input here

			var bldgProgram = new BldgProgram(input);
			var house = new House(input, bldgProgram);

			house.RunThrowAndStick();
			house.RunPushPull();

			foreach(Room r in house)
				Console.WriteLine($"{r.Name}'s coordinates are {r.Anchor}. Its dimensions are {r.Space}");

			house.Draw();
			Console.ReadKey();
		}
	}
}