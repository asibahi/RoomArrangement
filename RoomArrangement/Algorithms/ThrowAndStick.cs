using System;
using System.Collections.Generic;
using static System.Math;
using GAF;
using GAF.Operators;

namespace RoomArrangement
{
	// The name is short for Throw-Stuff-At-The-Wall-and-See-What-Sticks,
	// which is what the GA all really does.
	static class ThrowAndStick
	{
		// Using Genetic Algorithms
		public static void Run(int NumOfRooms)
		{
			var population = new Population(100, 9 * NumOfRooms, false, false);

			//create the genetic operators 
			var elite = new Elite(5);
			var crossover = new Crossover(0.85, true)
			{
				CrossoverType = CrossoverType.SinglePoint
			};
			var mutation = new BinaryMutate(0.08, true);

			//create the GA itself 
			var ga = new GeneticAlgorithm(population, CalculateFitness);

			//add the operators to the ga process pipeline 
			ga.Operators.Add(elite);
			ga.Operators.Add(crossover);
			ga.Operators.Add(mutation);

			// Events subscription
			ga.OnRunComplete += ga_OnRunComplete;
			ga.OnGenerationComplete += ga_OnGenerationComplete;

			// Run the GA 
			Console.WriteLine("Starting the GA");
			ga.Run(Terminate);

		}

		public static double CalculateFitness(Chromosome c)
		{
			var fitnessList = new List<double>();

			ReadChromosome(c);

			// Actual Evaluation
			for(int i = 0; i < Database.Count; i++)
			{
				for(int j = i; j < Database.Count; j++)
				{
					fitnessList.Add(CompareRooms(i, j));
				}
			}

			// Check for Boundary compliance
			foreach(Room r in Database.List)
			{
				var xFarthest = (r.Anchor.X + r.Space.XDimension - Database.Boundary.XDimension);
				if(xFarthest > 0)
					fitnessList.Add(Pow(GaussianFunc(xFarthest), 2));
				else
					fitnessList.Add(1d);

				var yFarthest = (r.Anchor.Y + r.Space.YDimension - Database.Boundary.YDimension);
				if(yFarthest > 0)
					fitnessList.Add(Pow(GaussianFunc(yFarthest), 2));
				else
					fitnessList.Add(1d);
			}


			fitnessList.Add(1d);
			// return fitnessList.Average();

			double fitness = 1;
			foreach(double d in fitnessList)
				fitness *= d;
			return fitness;
		}

		public static bool Terminate(Population population,
						int currentGeneration,
						long currentEvaluation)
						=> (population.MaximumFitness == 1);

		// It should compare whether two rooms intersect and if they are related, how far they are.
		static double CompareRooms(int i, int j)
		{
			var returnVal = 1d;
			if(i != j)
			{
				double xRec1, yRec1, xCnt1, yCnt1;
				double xRec2, yRec2, xCnt2, yCnt2;

				var ri = Database.List[i];
				var rj = Database.List[j];

				Database.ReadRoom(i, out xRec1, out yRec1, out xCnt1, out yCnt1);
				Database.ReadRoom(j, out xRec2, out yRec2, out xCnt2, out yCnt2);

				double xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
				double yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));

				// Related Rooms logic
				bool areRoomsRelated = Database.AreAdjacent(ri, rj);

				if(areRoomsRelated)
				{
					if((xDim == 0 && yDim < 0) || (xDim < 0 && yDim == 0))
						returnVal = 1;

					else if(xDim == 0 && yDim == 0)
						returnVal = GaussianFunc(1);

					else if((xDim > 0 && yDim < 0) || (xDim < 0 && yDim > 0))
						returnVal = GaussianFunc(Max(xDim, yDim));

					else
						returnVal = GaussianFunc(xDim) * GaussianFunc(yDim);

				}
				else
				{
					// Intersection logic
					if(xDim < 0 && yDim < 0)
						returnVal = GaussianFunc(xDim) * GaussianFunc(yDim);

				}
			}
			return returnVal;
		}

		static void ReadChromosome(Chromosome c)
		{
			// Assuming each chromosome represents a certain arrangmenet of THREE rooms
			// The chrome will have, for each room:
			// 4 bits for X location , 4 bits for Y location , 1 bit for Orientation
			//
			// Since we have three rooms for the proof of concept, each chromosome
			// will be 27 bits long. TWENTY SEVEN
			//
			// Each 9 bits is one room. A loop through the chromose should do it.
			//
			// Example Chromosome:	000100101001101010110101101
			// First Room:		000100101
			// Second Room:		001101010
			// Third Room:		110101101

			// Adjusting the Rooms
			for(int i = 0; i < c.Count; i += 9)
			{
				int x = Convert.ToInt32(c.ToBinaryString(i, 4), 2);
				int y = Convert.ToInt32(c.ToBinaryString(i + 4, 4), 2);
				int oTemp = Convert.ToInt32(c.ToBinaryString(i + 8, 1), 2);

				bool o = Convert.ToBoolean(oTemp);

				var j = i / 9;

				Database.List[j].Adjust(x, y, o);
			}
		}

		// Implementing the Gaussian Function (bell curve) when
		// a (peak)     = 1
		// b (center)   = 0
		// c (width)    = 1 because what the hell. It is the only thing that could/should be changed.
		// Check https://en.wikipedia.org/wiki/Gaussian_function and
		// http://www.wolframalpha.com/input/?i=f%5Cleft(x%5Cright)+%3D+e%5E%7B-+%7B+%5Cfrac%7B(x)%5E2+%7D%7B+2+*+1%5E2%7D+%7D+%7D
		// for details
		static double GaussianFunc(double x)
		{
			double a = 1;
			double b = 0;
			double c = 1;

			return (a * Pow(E, -(Pow((x - b), 2) / (2 * Pow(c, 2)))));
		}

		// Events subscription
		static void ga_OnGenerationComplete(object sender, GaEventArgs e)
		{
			var c = e.Population.GetTop(1)[0];
			Console.WriteLine("Fitness is {0}", c.Fitness);
		}
		static void ga_OnRunComplete(object sender, GaEventArgs e)
		{
			var c = e.Population.GetTop(1)[0];
			ReadChromosome(c);

			Console.WriteLine("The GA is Done");
			Console.WriteLine("Fitness is {0}", c.Fitness);


		}
	}
}