using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;

namespace RoomArrangement
{
	static class GeneticItems
	{
		private static double CalculateFitness(Chromosome c)
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

			var fitnessList = new List<double>();

			// Adjusting the Rooms
			for (int i = 0; i < c.Count; i += 9)
			{
				int x = Convert.ToInt32(c.ToBinaryString(i, 4), 2);
				int y = Convert.ToInt32(c.ToBinaryString(i + 4, 4), 2);
				int oTemp = Convert.ToInt32(c.ToBinaryString(i + 8, 1), 2);

				bool o = Convert.ToBoolean(oTemp);

				var j = i / 9;

				RoomDB.List[j].Adjust(x, y, o);
			}

			// Evaluate fitness for every couple so it is a fraction of 1 (1 being perfectly adjacent)
			// then add the individual fitness value to fitnessList

			for (int i =0; i < RoomDB.Count; i++)
			{

			}








			double fitness = 1;
			foreach (double d in fitnessList)
				fitness *= d;

			return fitness;
		}
	}
}
