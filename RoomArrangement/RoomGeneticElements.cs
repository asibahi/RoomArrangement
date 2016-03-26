using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using GAF;

namespace RoomArrangement
{
	static class GACompanions
	{
		public static double CalculateFitness(Chromosome c)
		{
			var fitnessList = new List<double>();

			ReadChromosome(c);

			// Actual Evaluation
			// STILL WORK IN PROGRESS

			for (int i = 0; i < Database.Count; i++)
			{
				for (int j = i; j < Database.Count; j++)
				{
					fitnessList.Add(CompareRooms(i, j));
				}
			}


			fitnessList.Add(1d);
			// return fitnessList.Average();

			double fitness = 1;
			foreach (double d in fitnessList)
				fitness *= d;
			return fitness;
		}

		public static bool Terminate(Population population,
						int currentGeneration,
						long currentEvaluation) => (population.MaximumFitness == 1);
		

		// I am still not sure what I should have this method return
		// It should compare whether two rooms intersect and if they are related, how far they are.
		private static double CompareRooms(int i, int j)
		{
			var returnVal = 1d;
			if (i != j)
			{
				double xRec1, yRec1, xCnt1, yCnt1;
				double xRec2, yRec2, xCnt2, yCnt2;

				var ri = Database.List[i];
				var rj = Database.List[j];

				ReadRoom(i, out xRec1, out yRec1, out xCnt1, out yCnt1);
				ReadRoom(j, out xRec2, out yRec2, out xCnt2, out yCnt2);

				double xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
				double yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));

				// Related Rooms logic
				bool areRoomsRelated = Database.AreAdjacent(ri, rj);

				if (areRoomsRelated)
				{
					if ((xDim == 0 && yDim < 0) || (xDim < 0 && yDim == 0))
						returnVal = 1;

					else if (xDim == 0 && yDim == 0)
						returnVal = GaussianFunc(1);

					else if ((xDim > 0 && yDim < 0) || (xDim < 0 && yDim > 0))
						returnVal = GaussianFunc(Max(xDim, yDim));

					else
						returnVal = GaussianFunc(xDim) * GaussianFunc(yDim);

				}
				else
				{
					// Intersection logic
					if (xDim < 0 && yDim < 0)
						returnVal = GaussianFunc(xDim) * GaussianFunc(yDim);

				}
			}
			return returnVal;
		}

		// sy's idea. makes the code "neater".
		private static void ReadRoom(int i,
					     out double recX,
					     out double recY,
					     out double cntX,
					     out double cntY)
		{
			var r = Database.List[i];
			recX = r.Space.XDimension;
			recY = r.Space.YDimension;
			cntX = r.Center.X;
			cntY = r.Center.Y;
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
			for (int i = 0; i < c.Count; i += 9)
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
		// c (width)    = 5 because what the hell. It is the only thing that could/should be changed.
		// Check https://en.wikipedia.org/wiki/Gaussian_function and
		// http://www.wolframalpha.com/input/?i=f%5Cleft(x%5Cright)+%3D+e%5E%7B-+%7B+%5Cfrac%7B(x)%5E2+%7D%7B+2+*+1%5E2%7D+%7D+%7D
		// for details
		private static double GaussianFunc(double x)
		{
			double a = 1;
			double b = 0;
			double c = 1;

			return (a * Pow(E, -(Pow((x - b), 2) / (2 * Pow(c, 2)))));
		}
	}
}