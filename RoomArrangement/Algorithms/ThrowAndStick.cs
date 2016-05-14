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
		// Each chromosome represents a certain arrangmenet of rooms
		// a chromosome will have 17 bits total for each room:
		// 8 bits for X location , 8 bits for Y location , 1 bit for Orientation
		// A loop through the chromose would adjust the rooms accordingly.
		static int bitsToAdjustRoom = 17;
		static int populationSize = 100;

		static double boundaryXDim;
		static double boundaryYDim;

		// Using Genetic Algorithms
		public static void RunThrowAndStick(this House house)
		{
			boundaryXDim = house.Boundary.XDim;
			boundaryYDim = house.Boundary.YDim;

			var population = new Population(populationSize, bitsToAdjustRoom * house.RoomCount, false, false);

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
			ga.OnGenerationComplete += (sender, e) => WriteLine($"Fitness is {e.Population.GetTop(1)[0].Fitness}");
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
			for(int i = 0; i < house.RoomCount; i++)
				for(int j = i; j < house.RoomCount; j++)
					fitnessList.Add(CompareRooms(i, j, house));

			// Check for Boundary compliance
			foreach(Room r in house)
			{
				var xFarthest = r.Anchor.X + r.Space.XDim - boundaryXDim;
				if(xFarthest > 0)
					fitnessList.Add(Pow(BellCurve(xFarthest), 10));
				else
					fitnessList.Add(1);

				var yFarthest = r.Anchor.Y + r.Space.YDim - boundaryYDim;
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
			var fullRoom = bitsToAdjustRoom - 1;
			var halfRoom = fullRoom / 2;

			// Adjusting the Rooms
			for(int i = 0; i < c.Count; i += bitsToAdjustRoom)
			{
				int x = ToInt32(c.ToBinaryString(i, halfRoom), 2);
				int y = ToInt32(c.ToBinaryString(i + halfRoom, halfRoom), 2);
				int oTemp = ToInt32(c.ToBinaryString(i + fullRoom, 1), 2);

				bool o = ToBoolean(oTemp);

				house[i / bitsToAdjustRoom].Adjust(x, y, o);
			}
		}

		static double BellCurve(double x)
		{
			const int peak = 1;
			const int center = 0;
			const double width = 100;
			return peak * Pow(E, -(Pow(x - center, 2) / (2 * Pow(width, 2))));
		}

		// Event subscription
		static void ga_OnRunComplete(object sender, GaEventArgs e, House house)
		{
			var c = e.Population.GetTop(1)[0];
			ReadChromosome(c, house);

			WriteLine("The GA is Done");
			WriteLine($"Final Fitness is {c.Fitness}");
		}
	}
}