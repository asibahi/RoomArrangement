using System.Collections.Generic;
using System.Linq;
using GAF;
using GAF.Operators;
using static System.Console;
using static System.Convert;
using static System.Math;

namespace RoomArrangement
{
	public static class ThrowAndStick
	{
		// Using Genetic Algorithms
		public static void RunThrowAndStick(this House house)
		{
			var population = new Population(100, 17 * house.Count, false, false);

			//create the genetic operators 
			var elite = new Elite(5);
			var crossover = new Crossover(0.85, true, CrossoverType.SinglePoint);
			var mutation = new BinaryMutate(0.2, true);

			//create the GA itself 
			var ga = new GeneticAlgorithm(population, chromosome => EvaluateFitness(chromosome, house));

			//add the operators to the ga process pipeline 
			ga.Operators.Add(elite);
			ga.Operators.Add(crossover);
			ga.Operators.Add(mutation);

			// Events subscription
			ga.OnGenerationComplete += ga_OnGenerationComplete;
			ga.OnRunComplete += (sender, e) => ga_OnRunComplete(sender, e, house);

			// Run the GA 
			WriteLine("Starting the GA");
			ga.Run(Terminate);
		}

		static double EvaluateFitness(Chromosome c, House house)
		{
			var fitnessList = new List<double>();

			ReadChromosome(c, house);

			// Actual Evaluation
			for(int i = 0; i < house.Count; i++)
				for(int j = i; j < house.Count; j++)
					fitnessList.Add(CompareRooms(i, j, house));

			// Check for Boundary compliance
			foreach(Room r in house)
			{
				var xFarthest = r.Anchor.X + r.Space.XDim - house.Boundary.XDim;
				if(xFarthest > 0)
					fitnessList.Add(Pow(BellCurve(xFarthest), 10));
				else
					fitnessList.Add(1);

				var yFarthest = r.Anchor.Y + r.Space.YDim - house.Boundary.YDim;
				if(yFarthest > 0)
					fitnessList.Add(Pow(BellCurve(yFarthest), 10));
				else
					fitnessList.Add(1);
			}

			fitnessList.Add(1);

			//return fitnessList.Aggregate((x, y) => x * y);
			return fitnessList.Average();
		}

		static bool Terminate(Population population,
						int currentGeneration,
						long currentEvaluation) => (population.MaximumFitness == 1) || currentGeneration == 5000;

		// It should compare whether two rooms intersect and if they are related, how far they are.
		static double CompareRooms(int i, int j, House house)
		{
			var returnVal = 1d;
			if(i != j)
			{
				double xRec1, yRec1, xCnt1, yCnt1;
				double xRec2, yRec2, xCnt2, yCnt2;

				var ri = house[i];
				var rj = house[j];

				ri.Read(out xRec1, out yRec1, out xCnt1, out yCnt1);
				rj.Read(out xRec2, out yRec2, out xCnt2, out yCnt2);

				double xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
				double yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));

				// Related Rooms logic
				if(house.AreAdjacent(ri, rj))
					if((xDim == 0 && yDim < 0) || (xDim < 0 && yDim == 0))
						returnVal = 1;
					else if(xDim == 0 && yDim == 0)
						returnVal = BellCurve(1);
					else if((xDim > 0 && yDim < 0) || (xDim < 0 && yDim > 0))
						returnVal = BellCurve(Max(xDim, yDim));
					else
						returnVal = BellCurve(xDim) * BellCurve(yDim);

				else if(xDim < 0 && yDim < 0)
					// Intersection logic 
					returnVal = BellCurve(xDim) * BellCurve(yDim);
			}
			return returnVal;
		}

		static void ReadChromosome(Chromosome c, House house)
		{
			// Each chromosome represents a certain arrangmenet of rooms
			// a chromosome will have 17 bits total for each room:
			// 8 bits for X location , 8 bits for Y location , 1 bit for Orientation
			// A loop through the chromose would adjust the rooms accordingly.
			//
			// Example with 9 bits instead of 17, but same idea:
			// Chromosome:		000100101_001101010_110101101
			// First Room:		0001_0010_1
			// Second Room:		0011_0101_0
			// Third Room:		1101_0110_1

			// Adjusting the Rooms
			for(int i = 0; i < c.Count; i += 17)
			{
				int x = ToInt32(c.ToBinaryString(i, 8), 2);
				int y = ToInt32(c.ToBinaryString(i + 8, 8), 2);
				int oTemp = ToInt32(c.ToBinaryString(i + 16, 1), 2);

				bool o = ToBoolean(oTemp);

				house[i / 17].Adjust(x, y, o);
			}
		}

		static double BellCurve(double x)
		{
			var a = 1; // peak value
			var b = 0; // center on x-axis
			var c = 100; // width of bell curve

			return (a * Pow(E, -(Pow((x - b), 2) / (2 * Pow(c, 2))))); // Gaussian Function
		}

		// Events subscription
		static double cMax;
		static void ga_OnGenerationComplete(object sender, GaEventArgs e)
		{
			var c = e.Population.GetTop(1)[0];
			if(c.Fitness > cMax)
			{
				cMax = c.Fitness;
				WriteLine($"Fitness is {cMax}");
			}
		}

		static void ga_OnRunComplete(object sender, GaEventArgs e, House house)
		{
			var c = e.Population.GetTop(1)[0];
			ReadChromosome(c, house);

			WriteLine("The GA is Done");
			WriteLine($"Fitness is {c.Fitness}");
		}
	}
}