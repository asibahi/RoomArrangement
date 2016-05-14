using static System.Console;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new Input();

			input.RequestInput();

			var bldgProgram = new BldgProgram(input);
			var house = new House(input, bldgProgram);

			house.RunThrowAndStick();
			house.RunPushPull();

			foreach(Room r in house)
				WriteLine(r.ToString());

			foreach(System.Tuple<Room, Room> pair in house.Adjacencies)
				WriteLine(pair.ToString());

			//house.Draw();
			ReadKey();
		}
	}
}