using static System.Console;

namespace RoomArrangement
{
	static class RequestInputPlaceholder
	{
		public static void RequestInput(this Input input)
		{
			//This should be made better and integrated with the GUI TODO
			string tempInput;
			int tempNumber;

		TheVeryBeginning:

			WriteLine("How many sons do you have? (Please use numbers.)");
		SonsInput:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.Sons = tempNumber;
				WriteLine(string.Format("You have {0} sons. \r\n", input.Sons));
			}
			else
			{
				WriteLine("Please write a number.");
				goto SonsInput;
			}

			WriteLine("How many daughters do you have? (Please use numbers.)");
		DaughtersInput:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.Daughters = tempNumber;
				WriteLine(string.Format("You have {0} daughters. \r\n", input.Daughters));
			}
			else
			{
				WriteLine("Please write a number.");
				goto DaughtersInput;
			}

			WriteLine("How many parents live in the house?");
		ParentInput:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.Parents = tempNumber;
				WriteLine(string.Format("There are {0} parents. \r\n", input.Parents));
			}
			else
			{
				WriteLine("Please write a number.");
				goto ParentInput;
			}

			WriteLine("How many grandparents live in the house?");
		GParentInput:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.Grandparents = tempNumber;
				WriteLine(string.Format("There are {0} grandparents. \r\n", input.Grandparents));
			}
			else
			{
				WriteLine("Please write a number.");
				goto GParentInput;
			}

			WriteLine(string.Format("There are {0} people in your family.", input.Total));
			WriteLine("Is this correct? (y/n)");

		Question:
			tempInput = ReadLine().ToLower();

			if(tempInput == "y" || tempInput == "yes")
			{
				WriteLine("Thank you. \r\n");
			}
			else if(tempInput == "n" || tempInput == "no")
			{
				WriteLine("Please revise your input.");
				goto TheVeryBeginning;
			}
			else
			{
				WriteLine("Please type 'y' or 'n'.");
				goto Question;
			}

		PlotQuestions:
			WriteLine("What is your plot width?");
		PlotWidth:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.PlotWidth = tempNumber;
				WriteLine(string.Format("Your plot width is {0}'. \r\n", input.PlotWidth));
			}
			else
			{
				WriteLine("Please write a number.");
				goto PlotWidth;
			}

			WriteLine("What is your plot depth?");
		PlotDepth:
			tempInput = ReadLine();

			if(int.TryParse(tempInput, out tempNumber))
			{
				input.PlotDepth = tempNumber;
				WriteLine(string.Format("Your plot depth is {0}'.", input.PlotDepth));
				WriteLine(" ");
			}
			else
			{
				WriteLine("Please write a number.");
				goto PlotDepth;
			}

			WriteLine(string.Format("Your plot area is {0} sqft.", input.PlotArea));
			WriteLine("Is this correct? (y/n)");

		Question2:
			tempInput = ReadLine().ToLower();

			if(tempInput == "y" || tempInput == "yes")
			{
				WriteLine("Thank you. \r\n");
			}
			else if(tempInput == "n" || tempInput == "no")
			{
				WriteLine("Please revise your input.");
				goto PlotQuestions;
			}
			else
			{
				WriteLine("Please type 'y' or 'n'.");
				goto Question2;
			}
		}
	}
}
