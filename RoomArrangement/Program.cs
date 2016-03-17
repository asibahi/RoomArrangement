using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;

namespace RoomArrangement
{
	class Program
	{
		static void Main(string[] args)
		{

			// Create the Rooms
			var NumOfRooms = 3;

			for (int i = 0; i < NumOfRooms; i++)
			{
				var r = new Room();
			}



			var population = new Population(100, 9 * NumOfRooms, false, false);

			//create the genetic operators 
			var elite = new Elite(5);
			var crossover = new Crossover(0.85, true)
			{
				CrossoverType = CrossoverType.SinglePoint
			};
			var mutation = new BinaryMutate(0.08, true);

			//create the GA itself 
			var ga = new GeneticAlgorithm(population, RoomGeneticElements.CalculateFitness);

			ga.OnRunComplete += ga_OnRunComplete;

			//add the operators to the ga process pipeline 
			ga.Operators.Add(elite);
			ga.Operators.Add(crossover);
			ga.Operators.Add(mutation);

			//run the GA 
			ga.Run(RoomGeneticElements.Terminate);

			Console.ReadKey();
		}

		// subscribing to the event of Completion
		static void ga_OnRunComplete(object sender, GaEventArgs e)
		{
			var fittest = e.Population.GetTop(1)[0];

		}
	}
}
