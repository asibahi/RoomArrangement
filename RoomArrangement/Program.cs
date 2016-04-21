using System;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new Input();
			string tempInput;
			int tempNumber;

		#region Input This should be made better and integrated with the GUI TODO
		TheVeryBeginning:

			Console.WriteLine("How many sons do you have? (Please use numbers.)");
		SonsInput:
			tempInput = Console.ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.Sons = tempNumber;
				Console.WriteLine(string.Format("You have {0} sons.", input.Sons));
				Console.WriteLine(" ");
			}
			else
			{
				Console.WriteLine("Please write a number.");
				goto SonsInput;
			}


			Console.WriteLine("How many daughters do you have? (Please use numbers.)");
		DaughtersInput:
			tempInput = Console.ReadLine();

			if(Int32.TryParse(tempInput, out tempNumber))
			{
				input.Daughters = tempNumber;
				Console.WriteLine(string.Format("You have {0} daughters.", input.Daughters));
				Console.WriteLine(" ");
			}
			else
			{
				Console.WriteLine("Please write a number.");
				goto DaughtersInput;
			}


			Console.WriteLine("How many parents live in the house?");
		ParentInput:
			tempInput = Console.ReadLine();

			if(Int32.TryParse(tempInput, out tempNumber))
			{
				input.Parents = tempNumber;
				Console.WriteLine(string.Format("There are {0} parents.", input.Parents));
				Console.WriteLine(" ");
			}
			else
			{
				Console.WriteLine("Please write a number.");
				goto ParentInput;
			}

			Console.WriteLine("How many grandparents live in the house?");
		GParentInput:
			tempInput = Console.ReadLine();

			if(Int32.TryParse(tempInput, out tempNumber))
			{
				input.Grandparents = tempNumber;
				Console.WriteLine(string.Format("There are {0} grandparents.", input.Grandparents));
				Console.WriteLine(" ");
			}
			else
			{
				Console.WriteLine("Please write a number.");
				goto GParentInput;
			}

			Console.WriteLine(string.Format("There are {0} people in your family.", input.Total));
			Console.WriteLine("Is this correct? (y/n)");
		Question:
			tempInput = Console.ReadLine().ToLower();

			if(tempInput == "y" || tempInput == "yes")
			{
				Console.WriteLine("Thank you.");
				Console.WriteLine(" ");

			}
			else if(tempInput == "n" || tempInput == "no")
			{
				Console.WriteLine("Tough.");
				Console.WriteLine("Please revise your input.");
				goto TheVeryBeginning;
			}
			else
			{
				Console.WriteLine("Please type 'y' or 'n'.");
				goto Question;
			}

			#endregion



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