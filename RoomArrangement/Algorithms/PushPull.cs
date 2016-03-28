using System;
using System.Collections;
using System.Collections.Generic;
using static System.Math;

namespace RoomArrangement
{
	static class PushPull
	{
		public static void Run()
		{

			var hardCount = 0;
			var softCount = 0;
			var softLim = Database.Count * (Database.Count - 1);
			var loopCount = 0;
			var hardLim = 50000;


			do
			{
				loopCount++;

				// PULL connected rooms LOOP

				for (int i = 0; i < Database.Count; i++)
				{
					var r1 = Database.List[i];
					var adjList = Database.GetAdjacentRooms(r1);

					for (int j = 0; j < adjList.Count; j++)
					{
						var r2 = adjList[j];

						// Explanation for the numbers:
						// On, Dir, and Align are switches. Dim is a distance.
						//
						// Dir   control the Direction.
						// Align fix when two rectangles intersect corner to corner
						// Dim   control the main distance along the X and Y axis
						// On    control which Dimension is valid

						var distance = new Vector3d(r2.Space.Center - r1.Space.Center);

						//double xRec1, yRec1, xCnt1, yCnt1;
						//double xRec2, yRec2, xCnt2, yCnt2;

						//var ri = Database.List[i];
						//var rj = Database.List[j];

						//Database.ReadRoom(i, out xRec1, out yRec1, out xCnt1, out yCnt1);
						//Database.ReadRoom(j, out xRec2, out yRec2, out xCnt2, out yCnt2);

						//double xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
						//double yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));

						var xDim = Math.Abs(distance.X) - (r1.Space.X.Length / 2 + r2.Space.X.Length / 2);
						var yDim = Math.Abs(distance.Y) - (r1.Space.Y.Length / 2 + r2.Space.Y.Length / 2);
						var xOn = 0;
						var yOn = 0;
						var xDir = 1;
						var yDir = 1;
						var xAlign = 0;
						var yAlign = 0;

						// This is just fancy number when xDim == yDim
						var random = new Random();

						// Switching the switches
						if (r2.Space.Center.X > r1.Space.Center.X)
							xDir = -1;
						else
							xDir = 1;

						if (r2.Space.Center.Y > r1.Space.Center.Y)
							yDir = -1;
						else
							yDir = 1;

						if (xDim > 0)
							xOn = 1;

						if (yDim > 0)
							yOn = 1;

						if (xDim > 0 && yDim > 0)
						{
							if (xDim > yDim)
								yAlign = 1;
							else if (yDim > xDim)
								xAlign = 1;
							else
							{
								if (random.Next(0, 100) % 2 == 0)
									yAlign = 1;
								else
									xAlign = 1;
							}

						}

						// The main function:
						if (xDim != 0 && yDim != 0 && (r1.ID != r2.ID))
						{

							// This is the main Vector's logic which moves dictates the Pull function.
							var tVx = (xDir * xAlign * RoundToGrid((r1.Space.X.Length / 2))) + (xDir * xDim * xOn);
							var tVy = (yDir * yAlign * RoundToGrid((r1.Space.Y.Length / 2))) + (yDir * yDim * yOn);

							var tV = new Vector3d(tVx, tVy, 0);

							r2.Space.Transform(Transform.Translation(tV));

							double tempNumber = (Math.Round(tV.Length * 100000)) / 100000;

							Print(string.Format("{0} moved {2}' to {1}", r2.Name, r1.Name, tempNumber));

							feet += tV.Length;
						}

						r1.AdjacentRooms[j] = r2;
						rooms[i] = r1;
					}
				}

				// PUSH LOOP

				for (int i = 0; i < rooms.Count; i++)
				{
					var r1 = rooms[i];
					for (int j = 0; j < rooms.Count; j++)
					{

						var r2 = rooms[j];
						var distance = new Vector3d(r2.Space.Center - r1.Space.Center);

						var xDim = (r1.Space.X.Length / 2 + r2.Space.X.Length / 2) - Math.Abs(distance.X);
						var yDim = (r1.Space.Y.Length / 2 + r2.Space.Y.Length / 2) - Math.Abs(distance.Y);
						var xOn = 0;
						var yOn = 0;
						var xDir = 1;
						var yDir = 1;

						// Switching the switches
						if (r2.Space.Center.X > r1.Space.Center.X)
							xDir = -1;
						else
							xDir = 1;

						if (r2.Space.Center.Y > r1.Space.Center.Y)
							yDir = -1;
						else
							yDir = 1;

						if (xDim < yDim)
							xOn = 1;
						else
							yOn = 1;


						// The main PUSH function

						if (xDim > 0 && yDim > 0 && (r1.ID != r2.ID))
						{
							softCount = 0;

							var tVx = xDir * xDim * xOn;
							var tVy = yDir * yDim * yOn;

							var tV = new Vector3d(tVx, tVy, 0);

							r2.Space.Transform(Transform.Translation(-tV / 2));
							r1.Space.Transform(Transform.Translation(tV / 2));

							double tempNumber = (Math.Round(tV.Length * 100000)) / 100000;

							Print(string.Format("{0} moved {2}' out of {1}", r2.Name, r1.Name, tempNumber));

							feet += tV.Length;

							rooms[j] = r2;
							rooms[i] = r1;
						}
						else
						{
							if (r1.ID != r2.ID)
							{
								softCount++;
							}
						}
					}
				}

				if (++hardCount > hardLim)
					break;
			}
			while (softCount <= softLim);

			var roundedFeet = Math.Round(feet / 10000) / 100;

			Print(loopCount.ToString() + " loops" + " with a total of " + roundedFeet.ToString() + " million feet");

			// End of PUSH LOOP






















		}
	}
}