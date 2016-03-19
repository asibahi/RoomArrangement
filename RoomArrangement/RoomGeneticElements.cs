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

                                Database.List[j].Adjust(x, y, o);
                        }

                        // Actual Evaluation

                        double rec1X, rec1Y, cnt1X, cnt1Y;
                        double rec2X, rec2Y, cnt2X, cnt2Y;

                        for (int i = 0; i < Database.Count; i++)
                        {
                                ReadRoom(i, out rec1X, out rec1Y, out cnt1X, out cnt1Y);

                                for (int j = 0; j < Database.Count; j++)
                                {
                                        ReadRoom(j, out rec2X, out rec2Y, out cnt2X, out cnt2Y);
                                }
                        }


                        fitnessList.Add(1d);
                        //return fitnessList.Average();

                        double fitness = 1;
                        foreach (double d in fitnessList)
                                fitness *= d;
                        return fitness;
                }

                public static bool Terminate(Population population,
                                                int currentGeneration,
                                                long currentEvaluation)
                {
                        var a = currentGeneration > 1000;
                        var b = population.MaximumFitness == 1;

                        return (a || b);
                }

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

                // I am still not sure what I should have this method return
                // It should compare whether two rooms intersect and if they are related, how far they are.
                private static double CompareRooms(int i, int j)
                {
                        double returnVal = -1;
                        if (i != j)
                        {
                                double xRec1, yRec1, xCnt1, yCnt1;
                                double xRec2, yRec2, xCnt2, yCnt2;

                                var ri = Database.List[i];
                                var rj = Database.List[j];

                                ReadRoom(i, out xRec1, out yRec1, out xCnt1, out yCnt1);
                                ReadRoom(j, out xRec2, out yRec2, out xCnt2, out yCnt2);

                                double xDis = Abs(xCnt1 - xCnt2);
                                double yDis = Abs(yCnt1 - yCnt2);
                                double xSize = (xRec1 / 2) + (xRec2 / 2);
                                double ySixe = (yRec1 / 2) + (yRec2 / 2);

                                // Intersection logic
                                if (xSize > xDis && ySixe > yDis)
                                {

                                }

                                if (ri.AdjacentRooms.Contains(rj))
                                {
                                }
                        }
                        return returnVal;
                }
        }
}
